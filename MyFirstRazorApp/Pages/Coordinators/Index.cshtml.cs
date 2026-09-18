using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Coordinators
{
    public class IndexModel : PageModel
    {
        private readonly ICoordinatorService _coordinatorService;

        public IndexModel(ICoordinatorService coordinatorService)
        {
            _coordinatorService = coordinatorService;
        }

        public IList<Coordinator> Coordinators { get; set; } = new List<Coordinator>();

        public async Task OnGetAsync()
        {
            try
            {
                Coordinators = await _coordinatorService.GetAllCoordinatorsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Coordinators = new List<Coordinator>();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _coordinatorService.DeleteCoordinatorAsync(id);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while deleting.");
                await OnGetAsync();
                return Page();
            }
        }
    }
}