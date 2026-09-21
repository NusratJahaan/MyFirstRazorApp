using MyFirstRazorApp.Models.StudentModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services
{
    public interface IStudentService
    {
        // Student lookups
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student?> GetStudentBySystemUserIdAsync(int systemUserId);

        // CRUD
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int id);

        // Enrollment
        Task<List<Course>> GetEnrolledCoursesAsync(int studentId);
        Task<List<Course>> GetAvailableCoursesAsync(int studentId);
        Task<bool> EnrollAsync(int studentId, int courseId);
        Task<bool> UnenrollAsync(int studentId, int courseId);
        Task<bool> IsEnrolledAsync(int studentId, int courseId);
    }
}