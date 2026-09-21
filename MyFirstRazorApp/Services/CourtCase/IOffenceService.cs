using MyFirstRazorApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    }
}