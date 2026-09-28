using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.CourtCase.Warrants
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IWarrantService _warrantService;
        private readonly IComplaintService _complaintService;

        public EditModel(IWarrantService warrantService, IComplaintService complaintService)
        {
            _warrantService = warrantService;
            _complaintService = complaintService;
        }

        [BindProperty]
        public Warrant Warrant { get; set; } = new Warrant();

        public Complaint Complaint { get; set; } = new Complaint();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var warrant = await _warrantService.GetWarrantByIdAsync(id);
            if (warrant == null) return NotFound();

            if (warrant.FinishedAndLocked)
                return RedirectToPage("./Details", new { id });

            Warrant = warrant;

            var complaint = await _complaintService.GetComplaintByIdAsync(warrant.ComplaintId);
            if (complaint == null) return NotFound();
            Complaint = complaint;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Warrant.CreatedBy");
            ModelState.Remove("Warrant.UpdatedBy");
            ModelState.Remove("Warrant.CreatedDate");
            ModelState.Remove("Warrant.UpdatedDate");

            if (!ModelState.IsValid)
            {
                Complaint = await _complaintService.GetComplaintByIdAsync(Warrant.ComplaintId) ?? new Complaint();
                return Page();
            }

            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "System";
                Warrant.UpdatedBy = userName;
                Warrant.UpdatedDate = DateTime.Now;

                await _warrantService.UpdateWarrantAsync(Warrant);
                return RedirectToPage("./List", new { complaintId = Warrant.ComplaintId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                Complaint = await _complaintService.GetComplaintByIdAsync(Warrant.ComplaintId) ?? new Complaint();
                return Page();
            }
        }
    }
}