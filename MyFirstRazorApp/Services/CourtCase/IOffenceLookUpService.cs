using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Services.CourtCase
{
    public interface IOffenceLookUpService
    {
        Task<List<OffenceLookUp>> GetAllAsync();
        Task<OffenceLookUp?> GetByIdAsync(int id);
    }
}