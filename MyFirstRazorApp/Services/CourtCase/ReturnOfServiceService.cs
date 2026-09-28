using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Services.CourtCase
{
    public class ReturnOfServiceService : IReturnOfServiceService
    {
        private readonly AppDbContext _context;

        public ReturnOfServiceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReturnOfService>> GetReturnsByWarrantIdAsync(int warrantId)
        {
            try
            {
                return await _context.ReturnsOfService
                    .Where(r => r.WarrantId == warrantId)
                    .OrderByDescending(r => r.Id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting returns: {ex.Message}", ex);
            }
        }

        public async Task<ReturnOfService?> GetReturnByIdAsync(int id)
        {
            try
            {
                return await _context.ReturnsOfService.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting return: {ex.Message}", ex);
            }
        }

        public async Task<bool> AddReturnAsync(ReturnOfService returnOfService)
        {
            try
            {
                //  Block if already served
                var alreadyServed = await _context.ReturnsOfService
                    .AnyAsync(r => r.WarrantId == returnOfService.WarrantId
                                && r.ServiceStatus == ServiceStatus.Served);

                if (alreadyServed)
                {
                    throw new Exception("Warrant is already served — no more returns allowed");
                }

                if (returnOfService.ServiceStatus == 0)
                {
                    returnOfService.ServiceStatus = ServiceStatus.Pending;
                }

                await _context.ReturnsOfService.AddAsync(returnOfService);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding return: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateReturnAsync(ReturnOfService returnOfService)
        {
            try
            {
                var existing = await _context.ReturnsOfService.FindAsync(returnOfService.Id);
                if (existing == null) return false;

                if (existing.ServiceStatus == ServiceStatus.Served)
                {
                    throw new Exception("Cannot update a served return");
                }

                existing.ServedDate = returnOfService.ServedDate;
                existing.ServiceStatus = returnOfService.ServiceStatus;
                existing.Remarks = returnOfService.Remarks;
                existing.UpdatedBy = returnOfService.UpdatedBy;
                existing.UpdatedDate = returnOfService.UpdatedDate;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating return: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteReturnAsync(int id)
        {
            try
            {
                var returnOfService = await _context.ReturnsOfService.FindAsync(id);
                if (returnOfService == null) return false;

                if (returnOfService.ServiceStatus == ServiceStatus.Served)
                {
                    throw new Exception("Cannot delete a served return");
                }

                _context.ReturnsOfService.Remove(returnOfService);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting return: {ex.Message}", ex);
            }
        }
    }
}