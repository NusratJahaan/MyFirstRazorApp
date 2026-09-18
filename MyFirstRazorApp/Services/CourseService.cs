using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            try
            {
                return await _context.Courses
                    .Include(c => c.Teacher)
                    .Include(c => c.Students)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting courses: {ex.Message}");
            }
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            try
            {
                return await _context.Courses
                    .Include(c => c.Teacher)
                    .Include(c => c.Students)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting course: {ex.Message}");
            }
        }

        public async Task<bool> AddCourseAsync(Course course)
        {
            try
            {
                await _context.Courses.AddAsync(course);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding course: {ex.Message}");
            }
        }

        public async Task<bool> UpdateCourseAsync(Course course)
        {
            try
            {
                _context.Courses.Update(course);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating course: {ex.Message}");
            }
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            try
            {
                var course = await _context.Courses.FindAsync(id);
                if (course != null)
                {
                    _context.Courses.Remove(course);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting course: {ex.Message}");
            }
        }
    }
}
