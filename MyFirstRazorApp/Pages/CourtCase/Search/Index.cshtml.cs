using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models.Dtos;
using MyFirstRazorApp.Services.CourtCase;

namespace MyFirstRazorApp.Pages.CourtCase.Search
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IOffenceService _offenceService;

        [BindProperty(SupportsGet = true)]
        public PendingOffenceSearchDto Search { get; set; } = new PendingOffenceSearchDto();
        public IndexModel(IOffenceService offenceService)
        {
            _offenceService = offenceService;
        }
        public async Task OnGetAsync()
        {
            bool hasFilter = !string.IsNullOrWhiteSpace(Search.FileNumber)
                          || !string.IsNullOrWhiteSpace(Search.DefendantName)
                          || (Search.OffenceLookUpId.HasValue && Search.OffenceLookUpId.Value > 0)
                          || Search.FromDate.HasValue
                          || Search.ToDate.HasValue;

            if (hasFilter)
            {
                Search.Results = await _offenceService.SearchPendingOffencesAsync(
                    Search.FileNumber,
                    Search.DefendantName,
                    Search.OffenceLookUpId,
                    Search.FromDate,
                    Search.ToDate);
            }
        }
    }
}