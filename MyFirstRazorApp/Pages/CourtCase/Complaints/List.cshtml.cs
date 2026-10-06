using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;

namespace MyFirstRazorApp.Pages.CourtCase.Complaints
{
    [Authorize]
    public class ListModel : PageModel
    {
        public string SearchTerm { get; set; }
        public IList<Complaint> Complaints { get; set; }

        private readonly IComplaintService _complaintService;
        public ListModel(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        public async Task OnGetAsync(string searchTerm)
        {
            Complaints = new List<Complaint>();
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