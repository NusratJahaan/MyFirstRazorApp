using MyFirstRazorApp.Models;
using MyFirstRazorApp.Models.Dtos;

namespace MyFirstRazorApp.Services.CourtCase
{
    public interface IOffenceService
    {
        Task<List<Offence>> GetOffencesByComplaintIdAsync(int complaintId);
        Task<Offence?> GetOffenceByIdAsync(int id);
        Task<bool> AddOffenceAsync(Offence offence);
        Task<bool> UpdateOffenceAsync(Offence offence);
        Task<bool> DeleteOffenceAsync(int id);
        Task<bool> ApproveOffenceAsync(int id);
        Task<bool> DeclineOffenceAsync(int id);
        Task<bool> ToggleActiveAsync(int id);
        Task<string> GenerateFileNumberAsync();
        Task<List<PendingOffenceDto>> GetPendingOffencesAsync();
        Task<List<PendingOffenceDto>> SearchPendingOffencesAsync(
            string fileNumber,
            string defendantName,
            int? offenceLookUpId,
            DateTime? fromDate,
            DateTime? toDate);
    }
}