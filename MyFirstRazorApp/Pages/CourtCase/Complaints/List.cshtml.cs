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
        public IList<PendingOffenceDto> PendingOffences { get; set; }
        private readonly IComplaintService _complaintService;
        private readonly IOffenceService _offenceService;
        public ListModel(IComplaintService complaintService, IOffenceService offenceService)
        {
            _complaintService = complaintService;
            _offenceService = offenceService;
        }

        public async Task OnGetAsync(string searchTerm)
        {
            Complaints = new List<Complaint>();
            PendingOffences = new List<PendingOffenceDto>();
            try
            {
                SearchTerm = searchTerm;
                Complaints = await _complaintService.SearchComplaintsAsync(searchTerm);
                PendingOffences = await _offenceService.GetPendingOffencesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Complaints = new List<Complaint>();
                PendingOffences = new List<PendingOffenceDto>();
            }
        }
    }
}