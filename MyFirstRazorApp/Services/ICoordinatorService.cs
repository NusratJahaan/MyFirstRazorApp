using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Services
{
    public interface ICoordinatorService
    {
        Task<List<Coordinator>> GetAllCoordinatorsAsync();
        Task<Coordinator?> GetCoordinatorByIdAsync(int id);
        Task<bool> AddCoordinatorAsync(Coordinator coordinator);
        Task<bool> UpdateCoordinatorAsync(Coordinator coordinator);
        Task<bool> DeleteCoordinatorAsync(int id);
    }
}