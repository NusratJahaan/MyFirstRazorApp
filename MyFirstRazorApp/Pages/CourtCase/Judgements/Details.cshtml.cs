using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System.Security.Claims;

namespace MyFirstRazorApp.Pages.CourtCase.Judgements
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IJudgementService _judgementService;
        private readonly IComplaintService _complaintService;
        private readonly IOffenceService _offenceService;
        private readonly IOffenceLookUpService _offenceLookUpService;

        public DetailsModel(
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
        public List<Judgement> BatchJudgements { get; set; } = new();
        public List<Offence> Offences { get; set; } = new();
        public List<OffenceLookUp> OffenceLookUps { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var judgement = await _judgementService.GetJudgementByIdAsync(id);
            if (judgement == null) return NotFound();

            var offence = await _offenceService.GetOffenceByIdAsync(judgement.OffenceId);
            if (offence == null) return NotFound();

            var complaint = await _complaintService.GetComplaintByIdAsync(offence.ComplaintId);
            if (complaint == null) return NotFound();
            Complaint = complaint;

            OffenceLookUps = await _offenceLookUpService.GetAllAsync();
            Offences = (await _offenceService.GetOffencesByComplaintIdAsync(complaint.Id)).ToList();

            // Load entire batch
            var batchDate = TruncateToSecond(judgement.CreatedDate);
            var allJudgements = await _judgementService.GetJudgementsByComplaintIdAsync(complaint.Id);
            BatchJudgements = allJudgements
                .Where(j => TruncateToSecond(j.CreatedDate) == batchDate
                         && j.JudgeName == judgement.JudgeName)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostFinalizeAsync(int id)
        {
            try
            {
                var judgement = await _judgementService.GetJudgementByIdAsync(id);
                if (judgement == null) return NotFound();

                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "System";

                //  Finalize all judgements in the batch
                var batchDate = TruncateToSecond(judgement.CreatedDate);
                var allJudgements = await _judgementService.GetJudgementsByComplaintIdAsync(judgement.OffenceId == 0 ? 0 : 0);
                // Reload based on complaint
                var offence = await _offenceService.GetOffenceByIdAsync(judgement.OffenceId);
                if (offence == null) return NotFound();

                allJudgements = await _judgementService.GetJudgementsByComplaintIdAsync(offence.ComplaintId);
                var batch = allJudgements
                    .Where(j => TruncateToSecond(j.CreatedDate) == batchDate
                             && j.JudgeName == judgement.JudgeName)
                    .ToList();

                foreach (var j in batch)
                {
                    if (!j.FinishedAndLocked)
                    {
                        await _judgementService.FinalizeJudgementAsync(j.Id, userName);
                    }
                }

                return RedirectToPage("./Details", new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToPage("./Details", new { id });
            }
        }

        public string GetOffenceFileNumber(int offenceId)
        {
            return Offences.FirstOrDefault(o => o.Id == offenceId)?.FileNumber ?? "—";
        }

        public string GetOffenceTypeName(int offenceId)
        {
            var offence = Offences.FirstOrDefault(o => o.Id == offenceId);
            if (offence == null) return "—";
            return OffenceLookUps.FirstOrDefault(l => l.Id == offence.OffenceLookUpId)?.Description ?? "—";
        }

        private DateTime TruncateToSecond(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }
    }
}