using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.CourtCase.Judgements
{
    [Authorize]
    public class AddModel : PageModel
    {
        private readonly IJudgementService _judgementService;
        private readonly IComplaintService _complaintService;
        private readonly IOffenceService _offenceService;

        public AddModel(
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
        public List<SelectListItem> OffenceOptions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int complaintId)
        {
            var complaint = await _complaintService.GetComplaintByIdAsync(complaintId);
            if (complaint == null) return NotFound();

            Complaint = complaint;
            await LoadOffenceOptionsAsync(complaintId);

            Judgement.JudgementDate = DateTime.Now;
            Judgement.SignedDate = DateTime.Now;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int complaintId)
        {
            ModelState.Remove("Judgement.CreatedBy");
            ModelState.Remove("Judgement.UpdatedBy");
            ModelState.Remove("Judgement.CreatedDate");
            ModelState.Remove("Judgement.UpdatedDate");

            Complaint = await _complaintService.GetComplaintByIdAsync(complaintId) ?? new Complaint();
            await LoadOffenceOptionsAsync(complaintId);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "System";
                Judgement.CreatedBy = userName;
                Judgement.UpdatedBy = userName;
                Judgement.CreatedDate = DateTime.Now;
                Judgement.UpdatedDate = DateTime.Now;

                await _judgementService.AddJudgementAsync(Judgement);
                return RedirectToPage("./List", new { complaintId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return Page();
            }
        }

        private async Task LoadOffenceOptionsAsync(int complaintId)
        {
            var offences = await _offenceService.GetOffencesByComplaintIdAsync(complaintId);
            OffenceOptions = offences
                .Where(o => o.IsApproved)
                .Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = $"{o.FileNumber}"
                }).ToList();
        }
    }
}