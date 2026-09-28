using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Services.CourtCase
{
    public class JudgementService : IJudgementService
    {
        private readonly AppDbContext _context;

        public JudgementService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Judgement>> GetJudgementsByComplaintIdAsync(int complaintId)
        {
            try
            {
                // Get all offence IDs for this complaint, then judgements for those offences
                var offenceIds = await _context.Offences
                    .Where(o => o.ComplaintId == complaintId)
                    .Select(o => o.Id)
                    .ToListAsync();

                return await _context.Judgements
                    .Where(j => offenceIds.Contains(j.OffenceId))
                    .OrderByDescending(j => j.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting judgements: {ex.Message}", ex);
            }
        }

        public async Task<List<Judgement>> GetJudgementsByOffenceIdAsync(int offenceId)
        {
            try
            {
                return await _context.Judgements
                    .Where(j => j.OffenceId == offenceId)
                    .OrderByDescending(j => j.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting judgements: {ex.Message}", ex);
            }
        }

        public async Task<Judgement?> GetJudgementByIdAsync(int id)
        {
            try
            {
                return await _context.Judgements.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting judgement: {ex.Message}", ex);
            }
        }

        public async Task<bool> AddJudgementAsync(Judgement judgement)
        {
            try
            {
                // Block if offence already has a finalized judgement
                var existingFinalized = await _context.Judgements
                    .AnyAsync(j => j.OffenceId == judgement.OffenceId && j.FinishedAndLocked);

                if (existingFinalized)
                {
                    throw new Exception("This offence already has a finalized judgement");
                }

                judgement.FinishedAndLocked = false;
                await _context.Judgements.AddAsync(judgement);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding judgement: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateJudgementAsync(Judgement judgement)
        {
            try
            {
                var existing = await _context.Judgements.FindAsync(judgement.Id);
                if (existing == null) return false;

                if (existing.FinishedAndLocked)
                {
                    throw new Exception("Cannot update a finalized judgement");
                }

                existing.JudgementDisposition = judgement.JudgementDisposition;
                existing.Description = judgement.Description;
                existing.JudgementDate = judgement.JudgementDate;
                existing.JudgeName = judgement.JudgeName;
                existing.SignedDate = judgement.SignedDate;
                existing.SignatureInfo = judgement.SignatureInfo;
                existing.UpdatedBy = judgement.UpdatedBy;
                existing.UpdatedDate = judgement.UpdatedDate;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating judgement: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteJudgementAsync(int id)
        {
            try
            {
                var judgement = await _context.Judgements.FindAsync(id);
                if (judgement == null) return false;

                if (judgement.FinishedAndLocked)
                {
                    throw new Exception("Cannot delete a finalized judgement");
                }

                _context.Judgements.Remove(judgement);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting judgement: {ex.Message}", ex);
            }
        }

        //  Finalize: lock judgement + mark offence finished + set case inactive
        public async Task<bool> FinalizeJudgementAsync(int id, string userName)
        {
            try
            {
                var judgement = await _context.Judgements.FindAsync(id);
                if (judgement == null) return false;

                if (judgement.FinishedAndLocked)
                {
                    throw new Exception("Judgement already finalized");
                }

                // Lock judgement
                judgement.FinishedAndLocked = true;
                judgement.UpdatedBy = userName;
                judgement.UpdatedDate = DateTime.Now;

                //  Cascade to Offence
                var offence = await _context.Offences.FindAsync(judgement.OffenceId);
                if (offence != null)
                {
                    offence.FinishedAndLocked = true;
                    offence.CaseStatus = CaseStatus.Inactive;
                    offence.UpdatedBy = userName;
                    offence.UpdatedDate = DateTime.Now;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error finalizing judgement: {ex.Message}", ex);
            }
        }
    }
}