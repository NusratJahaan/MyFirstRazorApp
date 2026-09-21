using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Models.StudentModels;
using MyFirstRazorApp.Services;

namespace MyFirstRazorApp.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; } = new RegisterInputModel();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Check if email already exists
                if (await _userService.EmailExistsAsync(Input.Email))
                {
                    ModelState.AddModelError("Input.Email", "Email already exists");
                    return Page();
                }

                // Build SystemUser from input
                var user = new SystemUser
                {
                    FullName = Input.FullName,
                    Email = Input.Email,
                    Role = Input.Role
                };

                // Save via service
                await _userService.RegisterAsync(user, Input.Password);

                return RedirectToPage("/Account/Login");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError("", "An error occurred. Please try again.");
                return Page();
            }
        }

        public class RegisterInputModel
        {
            [BindProperty]
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Full name is required")]
            [System.ComponentModel.DataAnnotations.StringLength(100, MinimumLength = 2)]
            [System.ComponentModel.DataAnnotations.Display(Name = "Full Name")]
            public string FullName { get; set; } = string.Empty;

            [BindProperty]
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Email is required")]
            [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Please enter a valid email")]
            [System.ComponentModel.DataAnnotations.Display(Name = "Email Address")]
            public string Email { get; set; } = string.Empty;

            [BindProperty]
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Role is required")]
            [System.ComponentModel.DataAnnotations.Display(Name = "Role")]
            public string Role { get; set; } = string.Empty;

            [BindProperty]
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Password is required")]
            [System.ComponentModel.DataAnnotations.StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
            [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
            [System.ComponentModel.DataAnnotations.Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;

            [BindProperty]
            [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
            [System.ComponentModel.DataAnnotations.Display(Name = "Confirm Password")]
            [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords do not match")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }
    }
}