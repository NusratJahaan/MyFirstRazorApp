using MyFirstRazorApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services.CourtCase
{
    public interface IOffenceLookUpService
    {
        Task<List<OffenceLookUp>> GetAllAsync();
        Task<OffenceLookUp?> GetByIdAsync(int id);
    }
}