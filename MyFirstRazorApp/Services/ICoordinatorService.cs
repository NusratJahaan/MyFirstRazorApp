using MyFirstRazorApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services
{
    public interface ICoordinatorService
    {
        Task<List<Coordinator>> GetAllCoordinatorsAsync();
        Task<Coordinator?> GetCoordinatorByIdAsync(int id);
        Task<Coordinator?> GetCoordinatorBySystemUserIdAsync(int systemUserId);
        Task AddCoordinatorAsync(Coordinator coordinator);
        Task UpdateCoordinatorAsync(Coordinator coordinator);
        Task DeleteCoordinatorAsync(int id);
    }
}