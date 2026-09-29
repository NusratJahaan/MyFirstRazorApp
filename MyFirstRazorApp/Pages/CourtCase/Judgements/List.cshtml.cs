using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.CourtCase.Judgements
{
    [Authorize]
    public class ListModel : PageModel
    {
        private readonly IJudgementService _judgementService;
        private readonly IComplaintService _complaintService;
        private readonly IOffenceService _offenceService;

        public ListModel(
            IJudgementService judgementService,
            IComplaintService complaintService,
            IOffenceService offenceService)
        {
            _judgementService = judgementService;
            _complaintService = complaintService;
            _offenceService = offenceService;
        }

        public List<JudgementBatch> JudgementBatches { get; set; } = new();
        public IList<Offence> Offences { get; set; } = new List<Offence>();
        public Complaint Complaint { get; set; } = new Complaint();

        public async Task<IActionResult> OnGetAsync(int complaintId)
        {
            var complaint = await _complaintService.GetComplaintByIdAsync(complaintId);
            if (complaint == null) return NotFound();

            Complaint = complaint;
            Offences = await _offenceService.GetOffencesByComplaintIdAsync(complaintId);

            var judgements = await _judgementService.GetJudgementsByComplaintIdAsync(complaintId);

            // ✅ Group by CreatedDate (rounded to the second) + JudgeName
            JudgementBatches = judgements
                .GroupBy(j => new
                {
                    BatchKey = new DateTime(
                        j.CreatedDate.Year,
                        j.CreatedDate.Month,
                        j.CreatedDate.Day,
                        j.CreatedDate.Hour,
                        j.CreatedDate.Minute,
                        j.CreatedDate.Second),
                    j.JudgeName
                })
                .Select(g => new JudgementBatch
                {
                    JudgementDate = g.First().JudgementDate,
                    JudgeName = g.Key.JudgeName,
                    FileNumbers = string.Join(", ", g.Select(j =>
                        Offences.FirstOrDefault(o => o.Id == j.OffenceId)?.FileNumber ?? "—")),
                    Count = g.Count(),
                    IsFinalized = g.All(j => j.FinishedAndLocked),
                    FirstJudgementId = g.First().Id,
                    OffenceIds = g.Select(j => j.OffenceId).ToList()
                })
                .OrderByDescending(b => b.JudgementDate)
                .ToList();

            return Page();
        }

        public string GetOffenceFileNumber(int offenceId)
        {
            return Offences.FirstOrDefault(o => o.Id == offenceId)?.FileNumber ?? "—";
        }
    }

    public class JudgementBatch
    {
        public DateTime JudgementDate { get; set; }
        public string JudgeName { get; set; }
        public string FileNumbers { get; set; }
        public int Count { get; set; }
        public bool IsFinalized { get; set; }
        public int FirstJudgementId { get; set; }
        public List<int> OffenceIds { get; set; } = new();
    }
}