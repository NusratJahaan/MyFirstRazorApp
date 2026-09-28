using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;

namespace MyFirstRazorApp.Pages.CourtCase.Witnesses
{
    [Authorize]
    public class ListModel : PageModel
    {
        private readonly IWitnessService _witnessService;
        private readonly IComplaintService _complaintService;

        public ListModel(IWitnessService witnessService, IComplaintService complaintService)
        {
            _witnessService = witnessService;
            _complaintService = complaintService;
        }

        public IList<Witness> Witnesses { get; set; } = new List<Witness>();
        public Complaint Complaint { get; set; } = new Complaint();

        public async Task<IActionResult> OnGetAsync(int complaintId)
        {
            try
            {
                var complaint = await _complaintService.GetComplaintByIdAsync(complaintId);
                if (complaint == null) return NotFound();

                Complaint = complaint;
                Witnesses = await _witnessService.GetWitnessesByComplaintIdAsync(complaintId);
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
                await _witnessService.DeleteWitnessAsync(id);
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