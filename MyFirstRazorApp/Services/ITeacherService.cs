using MyFirstRazorApp.Models.StudentModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services
{
    public interface ITeacherService
    {
        // Lookups
        Task<List<Teacher>> GetAllTeachersAsync();
        Task<Teacher?> GetTeacherByIdAsync(int id);
        Task<Teacher?> GetTeacherBySystemUserIdAsync(int systemUserId);

        // CRUD
        Task AddTeacherAsync(Teacher teacher);
        Task UpdateTeacherAsync(Teacher teacher);
        Task DeleteTeacherAsync(int id);

        // Course Claim / Release
        Task<List<Course>> GetMyCoursesAsync(int teacherId);
        Task<List<Course>> GetAvailableCoursesAsync();
        Task<bool> ClaimCourseAsync(int teacherId, int courseId);
        Task<bool> ReleaseCourseAsync(int teacherId, int courseId);

        // Students in a teacher's courses
        Task<List<Student>> GetStudentsInMyCoursesAsync(int teacherId);
        Task<List<Student>> GetStudentsInCourseAsync(int courseId);
    }
}