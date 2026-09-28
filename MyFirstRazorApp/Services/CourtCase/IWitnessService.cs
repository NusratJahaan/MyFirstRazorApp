using MyFirstRazorApp.Models;

namespace MyFirstRazorApp.Services.CourtCase
{
    public interface IWitnessService
    {
        Task<List<Witness>> GetWitnessesByComplaintIdAsync(int complaintId);
        Task<Witness?> GetWitnessByIdAsync(int id);
        Task<bool> AddWitnessAsync(Witness witness);
        Task<bool> UpdateWitnessAsync(Witness witness);
        Task<bool> DeleteWitnessAsync(int id);
    }
}