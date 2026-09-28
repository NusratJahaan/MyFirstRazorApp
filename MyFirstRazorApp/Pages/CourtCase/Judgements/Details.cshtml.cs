using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.CourtCase.Judgements
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IJudgementService _judgementService;
        private readonly IComplaintService _complaintService;
        private readonly IOffenceService _offenceService;

        public DetailsModel(
            IJudgementService judgementService,
            IComplaintService complaintService,
            IOffenceService offenceService)
        {
            _judgementService = judgementService;
            _complaintService = complaintService;
            _offenceService = offenceService;
        }

        public Judgement Judgement { get; set; } = new Judgement();
        public Complaint Complaint { get; set; } = new Complaint();
        public Offence Offence { get; set; } = new Offence();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var judgement = await _judgementService.GetJudgementByIdAsync(id);
            if (judgement == null) return NotFound();

            Judgement = judgement;

            var offence = await _offenceService.GetOffenceByIdAsync(judgement.OffenceId);
            if (offence == null) return NotFound();
            Offence = offence;

            var complaint = await _complaintService.GetComplaintByIdAsync(offence.ComplaintId);
            if (complaint == null) return NotFound();
            Complaint = complaint;

            return Page();
        }

        public async Task<IActionResult> OnPostFinalizeAsync(int id)
        {
            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "System";
                await _judgementService.FinalizeJudgementAsync(id, userName);
                return RedirectToPage("./Details", new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToPage("./Details", new { id });
            }
        }
    }
}