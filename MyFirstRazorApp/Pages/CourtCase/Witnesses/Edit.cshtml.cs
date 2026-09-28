using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.CourtCase.Witnesses
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IWitnessService _witnessService;
        private readonly IComplaintService _complaintService;

        public EditModel(IWitnessService witnessService, IComplaintService complaintService)
        {
            _witnessService = witnessService;
            _complaintService = complaintService;
        }

        [BindProperty]
        public Witness Witness { get; set; } = new Witness();

        public Complaint Complaint { get; set; } = new Complaint();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var witness = await _witnessService.GetWitnessByIdAsync(id);
            if (witness == null) return NotFound();

            Witness = witness;

            var complaint = await _complaintService.GetComplaintByIdAsync(witness.ComplaintId);
            if (complaint == null) return NotFound();
            Complaint = complaint;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Witness.CreatedBy");
            ModelState.Remove("Witness.UpdatedBy");
            ModelState.Remove("Witness.CreatedDate");
            ModelState.Remove("Witness.UpdatedDate");

            if (!ModelState.IsValid)
            {
                Complaint = await _complaintService.GetComplaintByIdAsync(Witness.ComplaintId) ?? new Complaint();
                return Page();
            }

            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "System";
                Witness.UpdatedBy = userName;
                Witness.UpdatedDate = DateTime.Now;

                await _witnessService.UpdateWitnessAsync(Witness);
                return RedirectToPage("./List", new { complaintId = Witness.ComplaintId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                Complaint = await _complaintService.GetComplaintByIdAsync(Witness.ComplaintId) ?? new Complaint();
                return Page();
            }
        }
    }
}