using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly AppDbContext _context;

        public TeacherService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            try
            {
                return await _context.Teachers
                    .Include(t => t.Course)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting teachers: {ex.Message}");
            }
        }

        public async Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            try
            {
                return await _context.Teachers
                    .Include(t => t.Course)
                    .FirstOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting teacher: {ex.Message}");
            }
        }

        public async Task<bool> AddTeacherAsync(Teacher teacher)
        {
            try
            {
                await _context.Teachers.AddAsync(teacher);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding teacher: {ex.Message}");
            }
        }

        public async Task<bool> UpdateTeacherAsync(Teacher teacher)
        {
            try
            {
                _context.Teachers.Update(teacher);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating teacher: {ex.Message}");
            }
        }

        public async Task<bool> DeleteTeacherAsync(int id)
        {
            try
            {
                var teacher = await _context.Teachers.FindAsync(id);
                if (teacher != null)
                {
                    _context.Teachers.Remove(teacher);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting teacher: {ex.Message}");
            }
        }
    }
}
