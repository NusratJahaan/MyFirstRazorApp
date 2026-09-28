using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Services.CourtCase
{
    public interface IReturnOfServiceService
    {
        Task<List<ReturnOfService>> GetReturnsByWarrantIdAsync(int warrantId);
        Task<ReturnOfService?> GetReturnByIdAsync(int id);
        Task<bool> AddReturnAsync(ReturnOfService returnOfService);
        Task<bool> UpdateReturnAsync(ReturnOfService returnOfService);
        Task<bool> DeleteReturnAsync(int id);
    }
}