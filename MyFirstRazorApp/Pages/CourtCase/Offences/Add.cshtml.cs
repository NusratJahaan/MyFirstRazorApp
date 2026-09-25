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
    public class AddModel : PageModel
    {
        private readonly IOffenceService _offenceService;
        private readonly IOffenceLookUpService _offenceLookUpService;
        private readonly IComplaintService _complaintService;

        public AddModel(
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

        public async Task<IActionResult> OnGetAsync(int complaintId)
        {
            try
            {
                var complaint = await _complaintService.GetComplaintByIdAsync(complaintId);
                if (complaint == null) return NotFound();

                Complaint = complaint;
                Offence.ComplaintId = complaintId;

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
            // ✅ Remove auto-managed fields
            ModelState.Remove("Offence.FileNumber");
            ModelState.Remove("Offence.CreatedBy");
            ModelState.Remove("Offence.UpdatedBy");
            ModelState.Remove("Offence.CreatedDate");
            ModelState.Remove("Offence.UpdatedDate");

            if (!ModelState.IsValid)
            {
                // Reload complaint + dropdown for redisplay
                Complaint = await _complaintService.GetComplaintByIdAsync(Offence.ComplaintId) ?? new Complaint();
                await LoadOffenceTypesAsync();
                return Page();
            }

            try
            {
                // ✅ Set audit from claims
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "System";
                Offence.CreatedBy = userName;
                Offence.UpdatedBy = userName;
                Offence.CreatedDate = DateTime.Now;
                Offence.UpdatedDate = DateTime.Now;

                await _offenceService.AddOffenceAsync(Offence);
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