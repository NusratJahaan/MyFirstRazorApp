using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.CourtCase.Complaints
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IComplaintService _complaintService;

        public DetailsModel(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        public Complaint Complaint { get; set; } = new Complaint();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var complaint = await _complaintService.GetComplaintByIdAsync(id);
            if (complaint == null)
            {
                return NotFound();
            }

            Complaint = complaint;
            return Page();
        }
    }
}