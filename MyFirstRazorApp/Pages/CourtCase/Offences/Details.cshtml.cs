using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.CourtCase.Offences
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IOffenceService _offenceService;
        private readonly IComplaintService _complaintService;
        private readonly IOffenceLookUpService _offenceLookUpService;

        public DetailsModel(
            IOffenceService offenceService,
            IComplaintService complaintService,
            IOffenceLookUpService offenceLookUpService)
        {
            _offenceService = offenceService;
            _complaintService = complaintService;
            _offenceLookUpService = offenceLookUpService;
        }

        public Offence Offence { get; set; } = new Offence();
        public Complaint Complaint { get; set; } = new Complaint();
        public string OffenceTypeName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var offence = await _offenceService.GetOffenceByIdAsync(id);
                if (offence == null) return NotFound();

                Offence = offence;

                var complaint = await _complaintService.GetComplaintByIdAsync(offence.ComplaintId);
                if (complaint == null) return NotFound();
                Complaint = complaint;

                var lookups = await _offenceLookUpService.GetAllAsync();
                var lookup = System.Linq.Enumerable.FirstOrDefault(lookups, l => l.Id == offence.OffenceLookUpId);
                OffenceTypeName = lookup?.Description ?? "Unknown";

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            try
            {
                var offence = await _offenceService.GetOffenceByIdAsync(id);
                if (offence == null) return NotFound();

                await _offenceService.ApproveOffenceAsync(id);
                return RedirectToPage("./Details", new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostDeclineAsync(int id)
        {
            try
            {
                var offence = await _offenceService.GetOffenceByIdAsync(id);
                if (offence == null) return NotFound();

                await _offenceService.DeclineOffenceAsync(id);
                return RedirectToPage("./Details", new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound();
            }
        }
    }
}