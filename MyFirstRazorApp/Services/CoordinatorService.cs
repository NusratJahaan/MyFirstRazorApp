using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services
{
    public class CoordinatorService : ICoordinatorService
    {
        private readonly AppDbContext _context;

        public CoordinatorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Coordinator>> GetAllCoordinatorsAsync()
        {
            try
            {
                return await _context.Coordinators
                    .Include(c => c.SystemUser)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting coordinators: {ex.Message}");
            }
        }

        public async Task<Coordinator?> GetCoordinatorByIdAsync(int id)
        {
            try
            {
                return await _context.Coordinators
                    .Include(c => c.SystemUser)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting coordinator: {ex.Message}");
            }
        }

        public async Task<Coordinator?> GetCoordinatorBySystemUserIdAsync(int systemUserId)
        {
            try
            {
                return await _context.Coordinators
                    .Include(c => c.SystemUser)
                    .FirstOrDefaultAsync(c => c.SystemUserId == systemUserId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting coordinator by user id: {ex.Message}");
            }
        }

        public async Task AddCoordinatorAsync(Coordinator coordinator)
        {
            try
            {
                coordinator.CreatedDate = DateTime.Now;
                coordinator.CreatedBy = "System";
                coordinator.UpdatedDate = DateTime.Now;
                coordinator.UpdatedBy = "System";

                await _context.Coordinators.AddAsync(coordinator);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding coordinator: {ex.Message}");
            }
        }

        public async Task UpdateCoordinatorAsync(Coordinator coordinator)
        {
            try
            {
                var existing = await _context.Coordinators.FindAsync(coordinator.Id);
                if (existing == null)
                {
                    throw new Exception("Coordinator not found");
                }

                existing.Name = coordinator.Name;
                existing.Email = coordinator.Email;
                existing.UpdatedDate = DateTime.Now;
                existing.UpdatedBy = "System";

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating coordinator: {ex.Message}");
            }
        }

        public async Task DeleteCoordinatorAsync(int id)
        {
            try
            {
                var coordinator = await _context.Coordinators.FindAsync(id);
                if (coordinator != null)
                {
                    _context.Coordinators.Remove(coordinator);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting coordinator: {ex.Message}");
            }
        }
    }
}