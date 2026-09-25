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
        //Suru te variable

        private readonly IComplaintService _complaintService;

        public AddModel(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        [BindProperty]
        public Complaint Complaint { get; set; } = new Complaint(); //object build kora jabe na

        public IActionResult OnGet()
        {
            //object build eikhane hbe
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
                return RedirectToPage($"{Navigator.Index}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding complaint: {ex.Message}", ex);
            }

            return Page();
        }
    }

}
