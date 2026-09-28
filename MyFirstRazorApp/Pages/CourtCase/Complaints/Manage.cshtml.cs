using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;

namespace MyFirstRazorApp.Pages.CourtCase.Complaints
{
    [Authorize]
    public class ManageModel : PageModel
    {
        private readonly IOffenceService _offenceService;
        private readonly IComplaintService _complaintService;
        private readonly IOffenceLookUpService _offenceLookUpService;

        public ManageModel(
            IOffenceService offenceService,
            IComplaintService complaintService,
            IOffenceLookUpService offenceLookUpService)
        {
            _offenceService = offenceService;
            _complaintService = complaintService;
            _offenceLookUpService = offenceLookUpService;
        }

        public IList<Offence> ActiveOffences { get; set; } = new List<Offence>();
        public IList<Offence> InactiveOffences { get; set; } = new List<Offence>();
        public IList<OffenceLookUp> OffenceLookUps { get; set; } = new List<OffenceLookUp>();
        public Complaint Complaint { get; set; } = new Complaint();

        public async Task<IActionResult> OnGetAsync(int complaintId)
        {
            var complaint = await _complaintService.GetComplaintByIdAsync(complaintId);
            if (complaint == null) return NotFound();

            Complaint = complaint;
            OffenceLookUps = await _offenceLookUpService.GetAllAsync();

            var offences = await _offenceService.GetOffencesByComplaintIdAsync(complaintId);

            ActiveOffences = offences.Where(o => o.CaseStatus == CaseStatus.Active).ToList();
            InactiveOffences = offences.Where(o => o.CaseStatus == CaseStatus.Inactive).ToList();

            return Page();
        }

        //  Toggle — flip the status
        public async Task<IActionResult> OnPostToggleAsync(int id, int complaintId)
        {
            try
            {
                await _offenceService.ToggleActiveAsync(id);
                return RedirectToPage("./Manage", new { complaintId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToPage("./Manage", new { complaintId });
            }
        }
    }
}