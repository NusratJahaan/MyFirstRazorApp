using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Services
{
    public interface ITeacherService
    {
        Task<List<Teacher>> GetAllTeachersAsync();
        Task<Teacher?> GetTeacherByIdAsync(int id);
        Task<bool> AddTeacherAsync(Teacher teacher);
        Task<bool> UpdateTeacherAsync(Teacher teacher);
        Task<bool> DeleteTeacherAsync(int id);
    }
}
