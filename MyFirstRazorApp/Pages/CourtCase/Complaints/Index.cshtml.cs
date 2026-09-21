using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;

namespace MyFirstRazorApp.Pages.CourtCase.Complaints
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IComplaintService _complaintService;

        public IndexModel(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        public IList<Complaint> Complaints { get; set; } = new List<Complaint>();
        public string? SearchTerm { get; set; }

        public async Task OnGetAsync(string? searchTerm)
        {
            try
            {
                SearchTerm = searchTerm;
                Complaints = await _complaintService.SearchComplaintsAsync(searchTerm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Complaints = new List<Complaint>();
            }
        }
    }
}