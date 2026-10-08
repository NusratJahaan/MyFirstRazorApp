using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstRazorApp.Models.Dtos;
using MyFirstRazorApp.Services.CourtCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.CourtCase.Search
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IOffenceService _offenceService;
        private readonly IOffenceLookUpService _offenceLookUpService;

        public IList<PendingOffenceDto> Results { get; set; }
        public List<SelectListItem> OffenceTypeOptions { get; set; }

        public string FileNumber { get; set; }
        public string DefendantName { get; set; }
        public int? OffenceLookUpId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public bool HasSearched { get; set; }

        public IndexModel(IOffenceService offenceService, IOffenceLookUpService offenceLookUpService)
        {
            _offenceService = offenceService;
            _offenceLookUpService = offenceLookUpService;
        }

        public async Task OnGetAsync(
            string fileNumber,
            string defendantName,
            int? offenceLookUpId,
            DateTime? fromDate,
            DateTime? toDate)
        {
            Results = new List<PendingOffenceDto>();
            OffenceTypeOptions = new List<SelectListItem>();

            var lookups = await _offenceLookUpService.GetAllAsync();
            OffenceTypeOptions = lookups
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Description
                })
                .ToList();

            FileNumber = fileNumber;
            DefendantName = defendantName;
            OffenceLookUpId = offenceLookUpId;
            FromDate = fromDate;
            ToDate = toDate;

            if (!string.IsNullOrWhiteSpace(fileNumber)
                || !string.IsNullOrWhiteSpace(defendantName)
                || (offenceLookUpId.HasValue && offenceLookUpId.Value > 0)
                || fromDate.HasValue
                || toDate.HasValue)
            {
                HasSearched = true;
                Results = await _offenceService.SearchPendingOffencesAsync(
                    fileNumber, defendantName, offenceLookUpId, fromDate, toDate);
            }
        }
    }
}