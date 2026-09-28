using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Services.CourtCase
{
    public class WitnessService : IWitnessService
    {
        private readonly AppDbContext _context;

        public WitnessService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Witness>> GetWitnessesByComplaintIdAsync(int complaintId)
        {
            try
            {
                return await _context.Witnesses
                    .Where(w => w.ComplaintId == complaintId)
                    .OrderBy(w => w.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting witnesses: {ex.Message}", ex);
            }
        }

        public async Task<Witness?> GetWitnessByIdAsync(int id)
        {
            try
            {
                return await _context.Witnesses.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting witness: {ex.Message}", ex);
            }
        }

        public async Task<bool> AddWitnessAsync(Witness witness)
        {
            try
            {
                await _context.Witnesses.AddAsync(witness);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding witness: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateWitnessAsync(Witness witness)
        {
            try
            {
                var existing = await _context.Witnesses.FindAsync(witness.Id);
                if (existing == null) return false;

                existing.Name = witness.Name;
                existing.Phone = witness.Phone;
                existing.Address = witness.Address;
                existing.IsVictim = witness.IsVictim;
                existing.Statement = witness.Statement;
                existing.UpdatedBy = witness.UpdatedBy;
                existing.UpdatedDate = witness.UpdatedDate;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating witness: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteWitnessAsync(int id)
        {
            try
            {
                var witness = await _context.Witnesses.FindAsync(id);
                if (witness == null) return false;

                _context.Witnesses.Remove(witness);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting witness: {ex.Message}", ex);
            }
        }
    }
}