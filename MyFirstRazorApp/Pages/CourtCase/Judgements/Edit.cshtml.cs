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
    public class EditModel : PageModel
    {
        private readonly IJudgementService _judgementService;
        private readonly IComplaintService _complaintService;
        private readonly IOffenceService _offenceService;
        private readonly IOffenceLookUpService _offenceLookUpService;

        public EditModel(
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
        public List<Judgement> BatchJudgements { get; set; } = new();
        public List<OffenceLookUp> OffenceLookUps { get; set; } = new();
        public List<int> ExistingOffenceIds { get; set; } = new();

        // Shared fields
        [BindProperty]
        public string JudgeName { get; set; } = string.Empty;

        [BindProperty]
        public DateTime JudgementDate { get; set; } = DateTime.Now;

        [BindProperty]
        public DateTime SignedDate { get; set; } = DateTime.Now;

        [BindProperty]
        public string SignatureInfo { get; set; } = string.Empty;

        // Row-aligned fields
        [BindProperty]
        public List<int> SelectedOffenceIds { get; set; } = new();

        [BindProperty]
        public List<int> RowOffenceIds { get; set; } = new();

        [BindProperty]
        public List<string> Dispositions { get; set; } = new();

        [BindProperty]
        public List<string> Descriptions { get; set; } = new();

        // Batch context
        [BindProperty]
        public DateTime BatchCreatedDate { get; set; }

        [BindProperty]
        public string BatchJudgeName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var judgement = await _judgementService.GetJudgementByIdAsync(id);
            if (judgement == null) return NotFound();

            if (judgement.FinishedAndLocked)
                return RedirectToPage("./Details", new { id });

            // ✅ Load the batch (same created-second + judge name)
            BatchCreatedDate = TruncateToSecond(judgement.CreatedDate);
            BatchJudgeName = judgement.JudgeName;

            var offence = await _offenceService.GetOffenceByIdAsync(judgement.OffenceId);
            if (offence == null) return NotFound();

            var complaint = await _complaintService.GetComplaintByIdAsync(offence.ComplaintId);
            if (complaint == null) return NotFound();
            Complaint = complaint;

            OffenceLookUps = await _offenceLookUpService.GetAllAsync();

            // Load all judgements in the batch
            var allJudgements = await _judgementService.GetJudgementsByComplaintIdAsync(complaint.Id);
            BatchJudgements = allJudgements
                .Where(j => TruncateToSecond(j.CreatedDate) == BatchCreatedDate
                         && j.JudgeName == BatchJudgeName)
                .ToList();

            ExistingOffenceIds = BatchJudgements.Select(j => j.OffenceId).ToList();

            // Prefill shared fields from first judgement
            JudgeName = judgement.JudgeName;
            JudgementDate = judgement.JudgementDate;
            SignedDate = judgement.SignedDate;
            SignatureInfo = judgement.SignatureInfo;

            // Load available offences (approved + not finished)
            var offences = await _offenceService.GetOffencesByComplaintIdAsync(complaint.Id);
            AvailableOffences = offences.Where(o => o.IsApproved && !o.FinishedAndLocked).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Judgement.CreatedBy");
            ModelState.Remove("Judgement.UpdatedBy");
            ModelState.Remove("Judgement.CreatedDate");
            ModelState.Remove("Judgement.UpdatedDate");

            if (SelectedOffenceIds == null)
                SelectedOffenceIds = new List<int>();

            // ✅ Load the FIRST judgement to find complaint context
            var firstIdStr = Request.Form["FirstJudgementId"].ToString();
            if (!int.TryParse(firstIdStr, out int firstId))
            {
                ModelState.AddModelError("", "Missing judgement reference.");
                return Page();
            }

            var firstJudgement = await _judgementService.GetJudgementByIdAsync(firstId);
            if (firstJudgement == null) return NotFound();

            var firstOffence = await _offenceService.GetOffenceByIdAsync(firstJudgement.OffenceId);
            if (firstOffence == null) return NotFound();

            Complaint = await _complaintService.GetComplaintByIdAsync(firstOffence.ComplaintId) ?? new Complaint();

            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "System";
                var now = DateTime.Now;

                // ✅ Load existing judgements for THIS batch (same created-second + judge)
                var batchDate = new DateTime(
                    firstJudgement.CreatedDate.Year,
                    firstJudgement.CreatedDate.Month,
                    firstJudgement.CreatedDate.Day,
                    firstJudgement.CreatedDate.Hour,
                    firstJudgement.CreatedDate.Minute,
                    firstJudgement.CreatedDate.Second);

                var allJudgements = await _judgementService.GetJudgementsByComplaintIdAsync(Complaint.Id);
                var batch = allJudgements
                    .Where(j => new DateTime(
                                    j.CreatedDate.Year, j.CreatedDate.Month, j.CreatedDate.Day,
                                    j.CreatedDate.Hour, j.CreatedDate.Minute, j.CreatedDate.Second) == batchDate
                             && j.JudgeName == firstJudgement.JudgeName)
                    .ToList();

                // ✅ DELETE unchecked judgements
                foreach (var existing in batch)
                {
                    if (!SelectedOffenceIds.Contains(existing.OffenceId))
                    {
                        await _judgementService.DeleteJudgementAsync(existing.Id);
                    }
                }

                // ✅ Loop through rows
                for (int i = 0; i < RowOffenceIds.Count; i++)
                {
                    var offenceId = RowOffenceIds[i];

                    if (!SelectedOffenceIds.Contains(offenceId)) continue;

                    var disposition = i < Dispositions.Count ? Dispositions[i] : "";
                    var description = i < Descriptions.Count ? Descriptions[i] : "";

                    if (string.IsNullOrWhiteSpace(disposition)) continue;

                    var existingJudgement = batch.FirstOrDefault(j => j.OffenceId == offenceId);

                    if (existingJudgement != null)
                    {
                        // ✅ UPDATE
                        existingJudgement.JudgementDisposition = disposition;
                        existingJudgement.Description = description;
                        existingJudgement.JudgeName = JudgeName;
                        existingJudgement.JudgementDate = JudgementDate;
                        existingJudgement.SignedDate = SignedDate;
                        existingJudgement.SignatureInfo = SignatureInfo;
                        existingJudgement.UpdatedBy = userName;
                        existingJudgement.UpdatedDate = now;

                        await _judgementService.UpdateJudgementAsync(existingJudgement);
                    }
                    else
                    {
                        // ✅ INSERT new — but with the ORIGINAL batch's CreatedDate to stay in same group
                        var newJudgement = new Judgement
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
                            CreatedDate = firstJudgement.CreatedDate,  // ✅ Same as batch!
                            UpdatedDate = now
                        };

                        await _judgementService.AddJudgementAsync(newJudgement);
                    }
                }

                return RedirectToPage("./List", new { complaintId = Complaint.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return Page();
            }
        }

        private DateTime TruncateToSecond(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }
    }
}