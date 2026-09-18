using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;

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
                return await _context.Coordinators.ToListAsync();
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
                return await _context.Coordinators.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting coordinator: {ex.Message}");
            }
        }

        public async Task<bool> AddCoordinatorAsync(Coordinator coordinator)
        {
            try
            {
                await _context.Coordinators.AddAsync(coordinator);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding coordinator: {ex.Message}");
            }
        }

        public async Task<bool> UpdateCoordinatorAsync(Coordinator coordinator)
        {
            try
            {
                _context.Coordinators.Update(coordinator);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating coordinator: {ex.Message}");
            }
        }

        public async Task<bool> DeleteCoordinatorAsync(int id)
        {
            try
            {
                var coordinator = await _context.Coordinators.FindAsync(id);
                if (coordinator != null)
                {
                    _context.Coordinators.Remove(coordinator);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting coordinator: {ex.Message}");
            }
        }
    }
}