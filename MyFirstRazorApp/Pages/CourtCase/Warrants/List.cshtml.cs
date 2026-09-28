using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;

namespace MyFirstRazorApp.Pages.CourtCase.Warrants
{
    [Authorize]
    public class ListModel : PageModel
    {
        private readonly IWarrantService _warrantService;
        private readonly IComplaintService _complaintService;

        public ListModel(IWarrantService warrantService, IComplaintService complaintService)
        {
            _warrantService = warrantService;
            _complaintService = complaintService;
        }

        public IList<Warrant> Warrants { get; set; } = new List<Warrant>();
        public Complaint Complaint { get; set; } = new Complaint();

        public async Task<IActionResult> OnGetAsync(int complaintId)
        {
            try
            {
                var complaint = await _complaintService.GetComplaintByIdAsync(complaintId);
                if (complaint == null) return NotFound();

                Complaint = complaint;
                Warrants = await _warrantService.GetWarrantsByComplaintIdAsync(complaintId);
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, int complaintId)
        {
            try
            {
                await _warrantService.DeleteWarrantAsync(id);
                return RedirectToPage("./List", new { complaintId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToPage("./List", new { complaintId });
            }
        }
    }
}