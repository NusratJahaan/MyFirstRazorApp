using MyFirstRazorApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services.CourtCase
{
    public interface IWarrantService
    {
        Task<List<Warrant>> GetWarrantsByComplaintIdAsync(int complaintId);
        Task<Warrant?> GetWarrantByIdAsync(int id);
        Task<bool> AddWarrantAsync(Warrant warrant);
        Task<bool> UpdateWarrantAsync(Warrant warrant);
        Task<bool> DeleteWarrantAsync(int id);
        Task<bool> LockWarrantAsync(int id, string userName);
    }
}