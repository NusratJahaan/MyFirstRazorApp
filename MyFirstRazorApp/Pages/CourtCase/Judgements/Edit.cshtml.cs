using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System.Security.Claims;

namespace MyFirstRazorApp.Pages.CourtCase.Judgements
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IJudgementService _judgementService;
        private readonly IComplaintService _complaintService;
        private readonly IOffenceService _offenceService;

        public EditModel(
            IJudgementService judgementService,
            IComplaintService complaintService,
            IOffenceService offenceService)
        {
            _judgementService = judgementService;
            _complaintService = complaintService;
            _offenceService = offenceService;
        }

        [BindProperty]
        public Judgement Judgement { get; set; } = new Judgement();

        public Complaint Complaint { get; set; } = new Complaint();
        public Offence Offence { get; set; } = new Offence();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var judgement = await _judgementService.GetJudgementByIdAsync(id);
            if (judgement == null) return NotFound();

            if (judgement.FinishedAndLocked)
                return RedirectToPage("./Details", new { id });

            Judgement = judgement;

            var offence = await _offenceService.GetOffenceByIdAsync(judgement.OffenceId);
            if (offence == null) return NotFound();
            Offence = offence;

            var complaint = await _complaintService.GetComplaintByIdAsync(offence.ComplaintId);
            if (complaint == null) return NotFound();
            Complaint = complaint;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Judgement.CreatedBy");
            ModelState.Remove("Judgement.UpdatedBy");
            ModelState.Remove("Judgement.CreatedDate");
            ModelState.Remove("Judgement.UpdatedDate");

            var existing = await _judgementService.GetJudgementByIdAsync(Judgement.Id);
            if (existing == null) return NotFound();

            var offence = await _offenceService.GetOffenceByIdAsync(existing.OffenceId);
            if (offence == null) return NotFound();
            Offence = offence;

            Complaint = await _complaintService.GetComplaintByIdAsync(offence.ComplaintId) ?? new Complaint();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "System";
                Judgement.UpdatedBy = userName;
                Judgement.UpdatedDate = DateTime.Now;

                await _judgementService.UpdateJudgementAsync(Judgement);
                return RedirectToPage("./List", new { complaintId = Complaint.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return Page();
            }
        }
    }
}