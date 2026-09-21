using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;


namespace MyFirstRazorApp.Pages.CourtCase.Complaints
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IComplaintService _complaintService;

        public CreateModel(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        [BindProperty]
        public Complaint Complaint { get; set; } = new Complaint();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _complaintService.AddComplaintAsync(Complaint);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError("", "An error occurred. Please try again.");
                return Page();
            }
        }
    }
}