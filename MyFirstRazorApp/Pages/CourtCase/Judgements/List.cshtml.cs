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

        public IList<Judgement> Judgements { get; set; } = new List<Judgement>();
        public IList<Offence> Offences { get; set; } = new List<Offence>();
        public Complaint Complaint { get; set; } = new Complaint();

        public async Task<IActionResult> OnGetAsync(int complaintId)
        {
            var complaint = await _complaintService.GetComplaintByIdAsync(complaintId);
            if (complaint == null) return NotFound();

            Complaint = complaint;
            Offences = await _offenceService.GetOffencesByComplaintIdAsync(complaintId);
            Judgements = await _judgementService.GetJudgementsByComplaintIdAsync(complaintId);
            return Page();
        }

        public string GetOffenceFileNumber(int offenceId)
        {
            return Offences.FirstOrDefault(o => o.Id == offenceId)?.FileNumber ?? "—";
        }
    }
}