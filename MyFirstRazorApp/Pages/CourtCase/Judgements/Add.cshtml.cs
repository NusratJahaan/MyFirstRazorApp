using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        private readonly IOffenceLookUpService _offenceLookUpService;

        public AddModel(
            IJudgementService judgementService,
            IComplaintService complaintService,
            IOffenceService offenceService,
            IOffenceLookUpService offenceLookUpService)
        {
            _judgementService = judgementService;
            _complaintService = complaintService;
            _offenceService = offenceService;
            _offenceLookUpService = offenceLookUpService;
        }

        public Complaint Complaint { get; set; } = new Complaint();
        public List<Offence> AvailableOffences { get; set; } = new();
        public List<OffenceLookUp> OffenceLookUps { get; set; } = new();

        // ✅ Shared fields
        [BindProperty]
        public string JudgeName { get; set; } = string.Empty;

        [BindProperty]
        public DateTime JudgementDate { get; set; } = DateTime.Now;

        [BindProperty]
        public DateTime SignedDate { get; set; } = DateTime.Now;

        [BindProperty]
        public string SignatureInfo { get; set; } = string.Empty;

        // ✅ Row-aligned fields (parallel lists)
        [BindProperty]
        public List<int> SelectedOffenceIds { get; set; } = new();

        [BindProperty]
        public List<int> RowOffenceIds { get; set; } = new();

        [BindProperty]
        public List<string> Dispositions { get; set; } = new();

        [BindProperty]
        public List<string> Descriptions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int complaintId)
        {
            var complaint = await _complaintService.GetComplaintByIdAsync(complaintId);
            if (complaint == null) return NotFound();

            Complaint = complaint;
            OffenceLookUps = await _offenceLookUpService.GetAllAsync();

            var offences = await _offenceService.GetOffencesByComplaintIdAsync(complaintId);
            AvailableOffences = offences.Where(o => o.IsApproved && !o.FinishedAndLocked).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int complaintId)
        {
            Complaint = await _complaintService.GetComplaintByIdAsync(complaintId) ?? new Complaint();
            OffenceLookUps = await _offenceLookUpService.GetAllAsync();

            var offences = await _offenceService.GetOffencesByComplaintIdAsync(complaintId);
            AvailableOffences = offences.Where(o => o.IsApproved && !o.FinishedAndLocked).ToList();

            // ✅ Validate at least one selected
            if (SelectedOffenceIds == null || !SelectedOffenceIds.Any())
            {
                ModelState.AddModelError("", "Please select at least one case.");
                return Page();
            }

            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "System";
                int created = 0;

                // ✅ Loop through row-aligned lists
                for (int i = 0; i < RowOffenceIds.Count; i++)
                {
                    var offenceId = RowOffenceIds[i];

                    // Skip unselected rows
                    if (!SelectedOffenceIds.Contains(offenceId)) continue;

                    var disposition = i < Dispositions.Count ? Dispositions[i] : "";
                    var description = i < Descriptions.Count ? Descriptions[i] : "";

                    // Skip if disposition is empty
                    if (string.IsNullOrWhiteSpace(disposition))
                    {
                        continue;
                    }

                    var judgement = new Judgement
                    {
                        OffenceId = offenceId,
                        JudgementDisposition = disposition,
                        Description = description,
                        JudgeName = JudgeName,
                        JudgementDate = JudgementDate,
                        SignedDate = SignedDate,
                        SignatureInfo = SignatureInfo,
                        FinishedAndLocked = false,
                        CreatedBy = userName,
                        UpdatedBy = userName,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };

                    await _judgementService.AddJudgementAsync(judgement);
                    created++;
                }

                if (created == 0)
                {
                    ModelState.AddModelError("", "Please enter disposition for at least one selected case.");
                    return Page();
                }

                return RedirectToPage("./List", new { complaintId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return Page();
            }
        }
    }
}