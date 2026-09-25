using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.CourtCase.Offences
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IOffenceService _offenceService;
        private readonly IOffenceLookUpService _offenceLookUpService;
        private readonly IComplaintService _complaintService;

        public EditModel(
            IOffenceService offenceService,
            IOffenceLookUpService offenceLookUpService,
            IComplaintService complaintService)
        {
            _offenceService = offenceService;
            _offenceLookUpService = offenceLookUpService;
            _complaintService = complaintService;
        }

        [BindProperty]
        public Offence Offence { get; set; } = new Offence();

        public Complaint Complaint { get; set; } = new Complaint();
        public List<SelectListItem> OffenceTypes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var offence = await _offenceService.GetOffenceByIdAsync(id);
                if (offence == null) return NotFound();

                Offence = offence;

                var complaint = await _complaintService.GetComplaintByIdAsync(offence.ComplaintId);
                if (complaint == null) return NotFound();
                Complaint = complaint;

                await LoadOffenceTypesAsync();
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
            // Remove auto-managed fields from validation
            ModelState.Remove("Offence.FileNumber");
            ModelState.Remove("Offence.CreatedBy");
            ModelState.Remove("Offence.UpdatedBy");
            ModelState.Remove("Offence.CreatedDate");
            ModelState.Remove("Offence.UpdatedDate");

            if (!ModelState.IsValid)
            {
                Complaint = await _complaintService.GetComplaintByIdAsync(Offence.ComplaintId) ?? new Complaint();
                await LoadOffenceTypesAsync();
                return Page();
            }

            try
            {
                // ✅ Update audit fields
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "System";
                Offence.UpdatedBy = userName;
                Offence.UpdatedDate = DateTime.Now;

                await _offenceService.UpdateOffenceAsync(Offence);
                return RedirectToPage("./List", new { complaintId = Offence.ComplaintId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("", $"Inner: {ex.InnerException.Message}");
                }
                Complaint = await _complaintService.GetComplaintByIdAsync(Offence.ComplaintId) ?? new Complaint();
                await LoadOffenceTypesAsync();
                return Page();
            }
        }

        private async Task LoadOffenceTypesAsync()
        {
            var types = await _offenceLookUpService.GetAllAsync();
            OffenceTypes = types.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = $"{t.Code} - {t.Description}"
            }).ToList();
        }
    }
}