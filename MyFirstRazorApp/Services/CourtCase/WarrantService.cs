using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Services.CourtCase
{
    public class WarrantService : IWarrantService
    {
        private readonly AppDbContext _context;

        public WarrantService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Warrant>> GetWarrantsByComplaintIdAsync(int complaintId)
        {
            try
            {
                return await _context.Warrants
                    .Where(w => w.ComplaintId == complaintId)
                    .OrderBy(w => w.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting warrants: {ex.Message}", ex);
            }
        }

        public async Task<Warrant?> GetWarrantByIdAsync(int id)
        {
            try
            {
                return await _context.Warrants.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting warrant: {ex.Message}", ex);
            }
        }

        public async Task<bool> AddWarrantAsync(Warrant warrant)
        {
            try
            {
                warrant.FinishedAndLocked = false;
                await _context.Warrants.AddAsync(warrant);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding warrant: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateWarrantAsync(Warrant warrant)
        {
            try
            {
                var existing = await _context.Warrants.FindAsync(warrant.Id);
                if (existing == null) return false;

                if (existing.FinishedAndLocked)
                {
                    throw new Exception("Cannot update a finished warrant");
                }

                existing.CaseNumbers = warrant.CaseNumbers;
                existing.IssuedDate = warrant.IssuedDate;
                existing.OfficialName = warrant.OfficialName;
                existing.OfficialSignDate = warrant.OfficialSignDate;
                existing.UpdatedBy = warrant.UpdatedBy;
                existing.UpdatedDate = warrant.UpdatedDate;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating warrant: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteWarrantAsync(int id)
        {
            try
            {
                var warrant = await _context.Warrants.FindAsync(id);
                if (warrant == null) return false;

                if (warrant.FinishedAndLocked)
                {
                    throw new Exception("Cannot delete a finished warrant");
                }

                _context.Warrants.Remove(warrant);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting warrant: {ex.Message}", ex);
            }
        }

        //  Lock the warrant (bypasses the FinishedAndLocked guard)
        public async Task<bool> LockWarrantAsync(int id, string userName)
        {
            try
            {
                var warrant = await _context.Warrants.FindAsync(id);
                if (warrant == null) return false;

                warrant.FinishedAndLocked = true;
                warrant.UpdatedBy = userName;
                warrant.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error locking warrant: {ex.Message}", ex);
            }
        }
    }
}