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
    public class AddModel : PageModel
    {
        private readonly IWarrantService _warrantService;
        private readonly IComplaintService _complaintService;

        public AddModel(IWarrantService warrantService, IComplaintService complaintService)
        {
            _warrantService = warrantService;
            _complaintService = complaintService;
        }

        [BindProperty]
        public Warrant Warrant { get; set; } = new Warrant();

        public Complaint Complaint { get; set; } = new Complaint();

        public async Task<IActionResult> OnGetAsync(int complaintId)
        {
            var complaint = await _complaintService.GetComplaintByIdAsync(complaintId);
            if (complaint == null) return NotFound();

            Complaint = complaint;
            Warrant.ComplaintId = complaintId;
            Warrant.IssuedDate = DateTime.Now;
            Warrant.OfficialSignDate = DateTime.Now;
            Warrant.CaseNumbers = complaint.ComplaintNumber;  // ✅ Auto-fill

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
                Warrant.CreatedBy = userName;
                Warrant.UpdatedBy = userName;
                Warrant.CreatedDate = DateTime.Now;
                Warrant.UpdatedDate = DateTime.Now;

                await _warrantService.AddWarrantAsync(Warrant);
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