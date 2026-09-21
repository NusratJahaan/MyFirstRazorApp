using MyFirstRazorApp.Models.StudentModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services
{
    public interface ICourseService
    {
        // Lookups
        Task<List<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(int id);
        Task<Course?> GetCourseWithDetailsAsync(int id);

        // CRUD (Coordinator only)
        Task<bool> AddCourseAsync(Course course);
        Task<bool> UpdateCourseAsync(Course course);
        Task<bool> DeleteCourseAsync(int id);

        // Course details for teacher-details page
        Task<List<Student>> GetStudentsInCourseAsync(int courseId);
    }
}