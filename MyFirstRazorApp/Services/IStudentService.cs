using MyFirstRazorApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<List<Student>> GetStudentsByCourseAsync(string courseName);
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int id);
    }
}