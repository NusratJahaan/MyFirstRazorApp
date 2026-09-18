using Microsoft.EntityFrameworkCore;
using MyFirstRazorApp.Data;
using MyFirstRazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services
{
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        // ---------- Lookups ----------

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            try
            {
                return await _context.Students
                    .Include(s => s.SystemUser)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting students: {ex.Message}");
            }
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            try
            {
                return await _context.Students
                    .Include(s => s.SystemUser)
                    .FirstOrDefaultAsync(s => s.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting student: {ex.Message}");
            }
        }

        public async Task<Student?> GetStudentBySystemUserIdAsync(int systemUserId)
        {
            try
            {
                return await _context.Students
                    .Include(s => s.SystemUser)
                    .FirstOrDefaultAsync(s => s.SystemUserId == systemUserId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting student by user id: {ex.Message}");
            }
        }

        // ---------- CRUD ----------

        public async Task AddStudentAsync(Student student)
        {
            try
            {
                student.CreatedDate = DateTime.Now;
                student.CreatedBy = "System";
                student.UpdatedDate = DateTime.Now;
                student.UpdatedBy = "System";

                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding student: {ex.Message}");
            }
        }

        public async Task UpdateStudentAsync(Student student)
        {
            try
            {
                var existing = await _context.Students.FindAsync(student.Id);
                if (existing == null)
                {
                    throw new Exception("Student not found");
                }

                existing.Name = student.Name;
                existing.Email = student.Email;
                existing.UpdatedDate = DateTime.Now;
                existing.UpdatedBy = "System";

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating student: {ex.Message}");
            }
        }

        public async Task DeleteStudentAsync(int id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student != null)
                {
                    _context.Students.Remove(student);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting student: {ex.Message}");
            }
        }

        // ---------- Enrollment ----------

        public async Task<List<Course>> GetEnrolledCoursesAsync(int studentId)
        {
            try
            {
                return await _context.StudentCourses
                    .Where(sc => sc.StudentId == studentId)
                    .Include(sc => sc.Course)
                        .ThenInclude(c => c!.Teacher)
                    .Select(sc => sc.Course!)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting enrolled courses: {ex.Message}");
            }
        }

        public async Task<List<Course>> GetAvailableCoursesAsync(int studentId)
        {
            try
            {
                // Courses with a teacher assigned, that student is NOT enrolled in
                var enrolledCourseIds = await _context.StudentCourses
                    .Where(sc => sc.StudentId == studentId)
                    .Select(sc => sc.CourseId)
                    .ToListAsync();

                return await _context.Courses
                    .Where(c => c.TeacherId != null && !enrolledCourseIds.Contains(c.Id))
                    .Include(c => c.Teacher)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting available courses: {ex.Message}");
            }
        }

        public async Task<bool> EnrollAsync(int studentId, int courseId)
        {
            try
            {
                // Check if already enrolled
                var exists = await _context.StudentCourses
                    .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

                if (exists) return false;

                var enrollment = new StudentCourse
                {
                    StudentId = studentId,
                    CourseId = courseId,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System",
                    UpdatedDate = DateTime.Now,
                    UpdatedBy = "System"
                };

                await _context.StudentCourses.AddAsync(enrollment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error enrolling: {ex.Message}");
            }
        }

        public async Task<bool> UnenrollAsync(int studentId, int courseId)
        {
            try
            {
                var enrollment = await _context.StudentCourses
                    .FirstOrDefaultAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);

                if (enrollment == null) return false;

                _context.StudentCourses.Remove(enrollment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error unenrolling: {ex.Message}");
            }
        }

        public async Task<bool> IsEnrolledAsync(int studentId, int courseId)
        {
            try
            {
                return await _context.StudentCourses
                    .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking enrollment: {ex.Message}");
            }
        }
    }
}