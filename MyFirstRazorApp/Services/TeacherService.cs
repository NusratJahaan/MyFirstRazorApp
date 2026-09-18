using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly AppDbContext _context;

        public TeacherService(AppDbContext context)
        {
            _context = context;
        }

        // ---------- Lookups ----------

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            try
            {
                return await _context.Teachers
                    .Include(t => t.SystemUser)
                    .Include(t => t.Courses)
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
                    .Include(t => t.SystemUser)
                    .Include(t => t.Courses)
                    .FirstOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting teacher: {ex.Message}");
            }
        }

        public async Task<Teacher?> GetTeacherBySystemUserIdAsync(int systemUserId)
        {
            try
            {
                return await _context.Teachers
                    .Include(t => t.SystemUser)
                    .Include(t => t.Courses)
                    .FirstOrDefaultAsync(t => t.SystemUserId == systemUserId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting teacher by user id: {ex.Message}");
            }
        }

        // ---------- CRUD ----------

        public async Task AddTeacherAsync(Teacher teacher)
        {
            try
            {
                teacher.CreatedDate = DateTime.Now;
                teacher.CreatedBy = "System";
                teacher.UpdatedDate = DateTime.Now;
                teacher.UpdatedBy = "System";

                await _context.Teachers.AddAsync(teacher);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding teacher: {ex.Message}");
            }
        }

        public async Task UpdateTeacherAsync(Teacher teacher)
        {
            try
            {
                var existing = await _context.Teachers.FindAsync(teacher.Id);
                if (existing == null)
                {
                    throw new Exception("Teacher not found");
                }

                existing.Name = teacher.Name;
                existing.Email = teacher.Email;
                existing.UpdatedDate = DateTime.Now;
                existing.UpdatedBy = "System";

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating teacher: {ex.Message}");
            }
        }

        public async Task DeleteTeacherAsync(int id)
        {
            try
            {
                var teacher = await _context.Teachers.FindAsync(id);
                if (teacher != null)
                {
                    _context.Teachers.Remove(teacher);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting teacher: {ex.Message}");
            }
        }

        // ---------- Course Claim / Release ----------

        public async Task<List<Course>> GetMyCoursesAsync(int teacherId)
        {
            try
            {
                return await _context.Courses
                    .Where(c => c.TeacherId == teacherId)
                    .Include(c => c.StudentCourses)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting my courses: {ex.Message}");
            }
        }

        public async Task<List<Course>> GetAvailableCoursesAsync()
        {
            try
            {
                // Courses with no teacher assigned yet
                return await _context.Courses
                    .Where(c => c.TeacherId == null)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting available courses: {ex.Message}");
            }
        }

        public async Task<bool> ClaimCourseAsync(int teacherId, int courseId)
        {
            try
            {
                var course = await _context.Courses.FindAsync(courseId);
                if (course == null || course.TeacherId != null)
                {
                    return false;  // Course doesn't exist or already claimed
                }

                course.TeacherId = teacherId;
                course.UpdatedDate = DateTime.Now;
                course.UpdatedBy = "System";

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error claiming course: {ex.Message}");
            }
        }

        public async Task<bool> ReleaseCourseAsync(int teacherId, int courseId)
        {
            try
            {
                var course = await _context.Courses.FindAsync(courseId);
                if (course == null || course.TeacherId != teacherId)
                {
                    return false;  // Not assigned to this teacher
                }

                course.TeacherId = null;
                course.UpdatedDate = DateTime.Now;
                course.UpdatedBy = "System";

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error releasing course: {ex.Message}");
            }
        }

        // ---------- Students ----------

        public async Task<List<Student>> GetStudentsInMyCoursesAsync(int teacherId)
        {
            try
            {
                var courseIds = await _context.Courses
                    .Where(c => c.TeacherId == teacherId)
                    .Select(c => c.Id)
                    .ToListAsync();

                var studentIds = await _context.StudentCourses
                    .Where(sc => courseIds.Contains(sc.CourseId))
                    .Select(sc => sc.StudentId)
                    .Distinct()
                    .ToListAsync();

                return await _context.Students
                    .Where(s => studentIds.Contains(s.Id))
                    .Include(s => s.SystemUser)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting students in my courses: {ex.Message}");
            }
        }

        public async Task<List<Student>> GetStudentsInCourseAsync(int courseId)
        {
            try
            {
                var studentIds = await _context.StudentCourses
                    .Where(sc => sc.CourseId == courseId)
                    .Select(sc => sc.StudentId)
                    .ToListAsync();

                return await _context.Students
                    .Where(s => studentIds.Contains(s.Id))
                    .Include(s => s.SystemUser)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting students in course: {ex.Message}");
            }
        }
    }
}