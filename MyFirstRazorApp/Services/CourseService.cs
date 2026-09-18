using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        // ---------- Lookups ----------

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            try
            {
                return await _context.Courses
                    .Include(c => c.Teacher)
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
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting course: {ex.Message}");
            }
        }

        public async Task<Course?> GetCourseWithDetailsAsync(int id)
        {
            try
            {
                return await _context.Courses
                    .Include(c => c.Teacher)
                    .Include(c => c.StudentCourses)
                        .ThenInclude(sc => sc.Student)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting course details: {ex.Message}");
            }
        }

        // ---------- CRUD ----------

        public async Task<bool> AddCourseAsync(Course course)
        {
            try
            {
                course.CreatedDate = DateTime.Now;
                course.CreatedBy = "System";
                course.UpdatedDate = DateTime.Now;
                course.UpdatedBy = "System";

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
                var existing = await _context.Courses.FindAsync(course.Id);
                if (existing == null)
                {
                    return false;
                }

                existing.Name = course.Name;
                existing.Description = course.Description;
                // TeacherId is NOT updated here — that's for Teacher claim/release
                existing.UpdatedDate = DateTime.Now;
                existing.UpdatedBy = "System";

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
                if (course == null)
                {
                    return false;
                }

                // Prevent deletion if students are enrolled
                var hasEnrollments = await _context.StudentCourses
                    .AnyAsync(sc => sc.CourseId == id);

                if (hasEnrollments)
                {
                    throw new Exception("Cannot delete course with enrolled students");
                }

                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting course: {ex.Message}");
            }
        }

        // ---------- Students in course ----------

        public async Task<List<Student>> GetStudentsInCourseAsync(int courseId)
        {
            try
            {
                return await _context.StudentCourses
                    .Where(sc => sc.CourseId == courseId)
                    .Include(sc => sc.Student)
                    .Select(sc => sc.Student!)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting students in course: {ex.Message}");
            }
        }
    }
}