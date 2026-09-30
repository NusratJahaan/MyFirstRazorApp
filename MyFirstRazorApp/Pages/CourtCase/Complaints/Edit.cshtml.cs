using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System.Security.Claims;

namespace MyFirstRazorApp.Pages.CourtCase.Complaints
{
    [Authorize]


    public class EditModel : PageModel
    {
        private readonly IComplaintService _complaintService;
        [BindProperty]
        public Complaint Complaint { get; set; }
        public EditModel(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Complaint = new Complaint();
            try
            {
                Complaint complaint = await _complaintService.GetComplaintByIdAsync(id);
                if (complaint == null)
                {
                    return NotFound();
                }

                Complaint = complaint;
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Complaint.ComplaintNumber");
            ModelState.Remove("Complaint.CreatedBy");
            ModelState.Remove("Complaint.CreatedDate");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                Complaint.UpdatedBy = userName;
                Complaint.UpdatedDate = DateTime.Now;
                await _complaintService.UpdateComplaintAsync(Complaint);
                return RedirectToPage("./List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", $"Inner: {ex.InnerException.Message}");
                }
                return Page();
            }
        }
    }
}