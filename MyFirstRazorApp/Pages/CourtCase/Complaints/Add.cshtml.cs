using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;

namespace MyFirstRazorApp.Pages.CourtCase.Complaints
{
    [Authorize]
    public class AddModel : PageModel
    {
        [BindProperty]
        public Complaint Complaint { get; set; }

        private readonly IComplaintService _complaintService;

        public AddModel(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        public IActionResult OnGet()
        {
            Complaint = new Complaint();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Complaint.ComplaintNumber");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _complaintService.AddComplaintAsync(Complaint);
                return Redirect("/CourtCase/Complaints/List");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding complaint: {ex.Message}", ex);
            }

            return Page();
        }
    }

}
