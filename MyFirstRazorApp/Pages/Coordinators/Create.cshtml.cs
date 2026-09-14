using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models;
using MyFirstRazorApp.Services;
using System;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Coordinators
{
    public class CreateModel : PageModel
    {
        private readonly ICoordinatorService _coordinatorService;

        public CreateModel(ICoordinatorService coordinatorService)
        {
            _coordinatorService = coordinatorService;
        }

        [BindProperty]
        public Coordinator Coordinator { get; set; } = new Coordinator();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _coordinatorService.AddCoordinatorAsync(Coordinator);
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