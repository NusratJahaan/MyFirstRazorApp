using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services.CourtCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.CourtCase.Warrants
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IWarrantService _warrantService;
        private readonly IComplaintService _complaintService;
        private readonly IReturnOfServiceService _returnOfServiceService;

        public DetailsModel(
            IWarrantService warrantService,
            IComplaintService complaintService,
            IReturnOfServiceService returnOfServiceService)
        {
            _warrantService = warrantService;
            _complaintService = complaintService;
            _returnOfServiceService = returnOfServiceService;
        }

        public Warrant Warrant { get; set; } = new Warrant();
        public Complaint Complaint { get; set; } = new Complaint();
        public IList<ReturnOfService> ReturnsOfService { get; set; } = new List<ReturnOfService>();

        public bool CanAddRos { get; set; } = false;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var warrant = await _warrantService.GetWarrantByIdAsync(id);
            if (warrant == null) return NotFound();

            Warrant = warrant;

            var complaint = await _complaintService.GetComplaintByIdAsync(warrant.ComplaintId);
            if (complaint == null) return NotFound();
            Complaint = complaint;

            ReturnsOfService = await _returnOfServiceService.GetReturnsByWarrantIdAsync(id);

            // ✅ Can add ROS only if warrant NOT locked AND no ROS is Served
            var anyServed = ReturnsOfService.Any(r => r.ServiceStatus == ServiceStatus.Served);
            CanAddRos = !anyServed;

            return Page();
        }

        // ✅ Finish & Lock the Warrant
        public async Task<IActionResult> OnPostLockAsync(int id)
        {
            try
            {
                var userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "System";
                await _warrantService.LockWarrantAsync(id, userName);
                return RedirectToPage("./Details", new { id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToPage("./Details", new { id });
            }
        }
    }
}