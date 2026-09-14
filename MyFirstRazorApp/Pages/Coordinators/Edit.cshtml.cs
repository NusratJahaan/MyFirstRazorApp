using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;
using System;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Coordinators
{
    public class EditModel : PageModel
    {
        private readonly ICoordinatorService _coordinatorService;

        public EditModel(ICoordinatorService coordinatorService)
        {
            _coordinatorService = coordinatorService;
        }

        [BindProperty]
        public Coordinator Coordinator { get; set; } = new Coordinator();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Coordinator = await _coordinatorService.GetCoordinatorByIdAsync(id) ?? new Coordinator();

                if (Coordinator.Id == 0)
                {
                    return NotFound();
                }

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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var existing = await _coordinatorService.GetCoordinatorByIdAsync(Coordinator.Id);

                if (existing == null)
                {
                    return NotFound();
                }

                existing.Name = Coordinator.Name;
                existing.Email = Coordinator.Email;
                existing.Phone = Coordinator.Phone;
                existing.Department = Coordinator.Department;
                existing.UpdatedDate = DateTime.Now;
                existing.UpdatedBy = "User";

                await _coordinatorService.UpdateCoordinatorAsync(existing);
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
