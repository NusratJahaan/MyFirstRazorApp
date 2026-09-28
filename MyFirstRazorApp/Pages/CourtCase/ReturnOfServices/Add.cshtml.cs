using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.CourtCase.ReturnOfServices
{
    [Authorize]
    public class AddModel : PageModel
    {
        private readonly IReturnOfServiceService _returnOfServiceService;
        private readonly IWarrantService _warrantService;
        private readonly IComplaintService _complaintService;

        public AddModel(
            IReturnOfServiceService returnOfServiceService,
            IWarrantService warrantService,
            IComplaintService complaintService)
        {
            _returnOfServiceService = returnOfServiceService;
            _warrantService = warrantService;
            _complaintService = complaintService;
        }

        [BindProperty]
        public ReturnOfService ReturnOfService { get; set; } = new ReturnOfService();

        public Warrant Warrant { get; set; } = new Warrant();
        public Complaint Complaint { get; set; } = new Complaint();

        public async Task<IActionResult> OnGetAsync(int warrantId)
        {
            var warrant = await _warrantService.GetWarrantByIdAsync(warrantId);
            if (warrant == null) return NotFound();

            Warrant = warrant;

            var complaint = await _complaintService.GetComplaintByIdAsync(warrant.ComplaintId);
            if (complaint == null) return NotFound();
            Complaint = complaint;

            ReturnOfService.WarrantId = warrantId;
            ReturnOfService.ServedDate = DateTime.Now;
            ReturnOfService.ServiceStatus = ServiceStatus.Pending;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("ReturnOfService.CreatedBy");
            ModelState.Remove("ReturnOfService.UpdatedBy");
            ModelState.Remove("ReturnOfService.CreatedDate");
            ModelState.Remove("ReturnOfService.UpdatedDate");

            var warrant = await _warrantService.GetWarrantByIdAsync(ReturnOfService.WarrantId);
            if (warrant == null) return NotFound();
            Warrant = warrant;

            Complaint = await _complaintService.GetComplaintByIdAsync(warrant.ComplaintId) ?? new Complaint();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "System";
                ReturnOfService.CreatedBy = userName;
                ReturnOfService.UpdatedBy = userName;
                ReturnOfService.CreatedDate = DateTime.Now;
                ReturnOfService.UpdatedDate = DateTime.Now;

                await _returnOfServiceService.AddReturnAsync(ReturnOfService);
                return RedirectToPage("/CourtCase/Warrants/Details", new { id = ReturnOfService.WarrantId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return Page();
            }
        }
    }
}