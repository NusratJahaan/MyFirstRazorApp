using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.CourtCase.Offences
{
    [Authorize]
    public class ListModel : PageModel
    {
        private readonly IOffenceService _offenceService;
        private readonly IComplaintService _complaintService;
        private readonly IOffenceLookUpService _offenceLookUpService;

        public ListModel(
            IOffenceService offenceService,
            IComplaintService complaintService,
            IOffenceLookUpService offenceLookUpService)
        {
            _offenceService = offenceService;
            _complaintService = complaintService;
            _offenceLookUpService = offenceLookUpService;
        }

        public IList<Offence> Offences { get; set; } = new List<Offence>();
        public IList<OffenceLookUp> OffenceLookUps { get; set; } = new List<OffenceLookUp>();
        public Complaint Complaint { get; set; } = new Complaint();

        public async Task<IActionResult> OnGetAsync(int complaintId)
        {
            try
            {
                var complaint = await _complaintService.GetComplaintByIdAsync(complaintId);
                if (complaint == null) return NotFound();

                Complaint = complaint;
                Offences = await _offenceService.GetOffencesByComplaintIdAsync(complaintId);
                OffenceLookUps = await _offenceLookUpService.GetAllAsync();
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostApproveAsync(int id, int complaintId)
        {
            try
            {
                await _offenceService.ApproveOffenceAsync(id);
                return RedirectToPage("./List", new { complaintId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToPage("./List", new { complaintId });
            }
        }

        public async Task<IActionResult> OnPostDeclineAsync(int id, int complaintId)
        {
            try
            {
                await _offenceService.DeclineOffenceAsync(id);
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