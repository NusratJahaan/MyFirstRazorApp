using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models.ViewModels;

namespace MyFirstRazorApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginViewModel Input { get; set; } = new LoginViewModel();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // TODO: Authentication logic will be added by mentor
            return RedirectToPage("/Index");
        }
    }
}