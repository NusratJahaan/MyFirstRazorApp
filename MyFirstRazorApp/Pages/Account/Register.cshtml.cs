using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models.ViewModels;

namespace MyFirstRazorApp.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterViewModel Input { get; set; } = new RegisterViewModel();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Save to Account Service and redirect to login page
            // TODO: Registration logic will be added by mentor
            return RedirectToPage("/Account/Login");
        }
    }
}