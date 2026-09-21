using MyFirstRazorApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services.CourtCase
{
    public interface IComplaintService
    {
        Task<List<Complaint>> GetAllComplaintsAsync();
        Task<Complaint?> GetComplaintByIdAsync(int id);
        Task<List<Complaint>> SearchComplaintsAsync(string? searchTerm);
        Task<bool> AddComplaintAsync(Complaint complaint);
        Task<bool> UpdateComplaintAsync(Complaint complaint);
        Task<bool> DeleteComplaintAsync(int id);
        Task<string> GenerateComplaintNumberAsync();
    }
}