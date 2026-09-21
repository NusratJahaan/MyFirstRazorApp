using MyFirstRazorApp.Models.StudentModels;

namespace MyFirstRazorApp.Services
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(SystemUser user, string plainPassword);
        Task<SystemUser?> ValidateUserAsync(string email, string password);
        Task<bool> EmailExistsAsync(string email);
    }
}