using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services.CourtCase
{
    public class OffenceService : IOffenceService
    {
        private readonly AppDbContext _context;

        public OffenceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Offence>> GetOffencesByComplaintIdAsync(int complaintId)
        {
            try
            {
                return await _context.Offences
                    .Where(o => o.ComplaintId == complaintId)
                    .OrderBy(o => o.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting offences: {ex.Message}");
            }
        }

        public async Task<Offence?> GetOffenceByIdAsync(int id)
        {
            try
            {
                return await _context.Offences.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting offence: {ex.Message}");
            }
        }

        public async Task<bool> AddOffenceAsync(Offence offence)
        {
            try
            {
                offence.OffenceStatus = OffenceStatus.Pending;
                offence.CaseStatus = CaseStatus.Active;
                offence.IsApproved = false;
                offence.FinishedAndLocked = false;

                await _context.Offences.AddAsync(offence);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding offence: {ex.Message}");
            }
        }

        public async Task<bool> UpdateOffenceAsync(Offence offence)
        {
            try
            {
                var existing = await _context.Offences.FindAsync(offence.Id);
                if (existing == null) return false;

                if (existing.FinishedAndLocked)
                {
                    throw new Exception("Cannot update a finished offence");
                }

                existing.OffenceLookUpId = offence.OffenceLookUpId;
                existing.Description = offence.Description;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating offence: {ex.Message}");
            }
        }

        public async Task<bool> DeleteOffenceAsync(int id)
        {
            try
            {
                var offence = await _context.Offences.FindAsync(id);
                if (offence == null) return false;

                if (offence.IsApproved)
                {
                    throw new Exception("Cannot delete an approved offence");
                }

                _context.Offences.Remove(offence);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting offence: {ex.Message}");
            }
        }

        public async Task<bool> ApproveOffenceAsync(int id)
        {
            try
            {
                var offence = await _context.Offences.FindAsync(id);
                if (offence == null) return false;

                offence.IsApproved = true;
                offence.OffenceStatus = OffenceStatus.Approved;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error approving offence: {ex.Message}");
            }
        }

        public async Task<bool> DeclineOffenceAsync(int id)
        {
            try
            {
                var offence = await _context.Offences.FindAsync(id);
                if (offence == null) return false;

                offence.IsApproved = false;
                offence.OffenceStatus = OffenceStatus.Declined;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error declining offence: {ex.Message}");
            }
        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            try
            {
                var offence = await _context.Offences.FindAsync(id);
                if (offence == null) return false;

                offence.CaseStatus = offence.CaseStatus == CaseStatus.Active
                    ? CaseStatus.Inactive
                    : CaseStatus.Active;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error toggling offence status: {ex.Message}");
            }
        }
    }
}