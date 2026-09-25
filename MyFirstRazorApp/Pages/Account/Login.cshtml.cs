using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Services;


namespace MyFirstRazorApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public LoginModel(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [BindProperty]
        public LoginInputModel Input { get; set; } = new LoginInputModel();

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
                //Validate user against DB
                var user = await _userService.ValidateUserAsync(Input.Email, Input.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return Page();
                }

                //Delegate to AuthService
                await _authService.SetupAuthClaims(user, HttpContext);

                return RedirectToPage("/CourtCase/Complaints/List");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError("", "An error occurred. Please try again.");
                return Page();
            }
        }

        public class LoginInputModel
        {
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Email is required")]
            [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Please enter a valid email")]
            [System.ComponentModel.DataAnnotations.Display(Name = "Email Address")]
            public string Email { get; set; } = string.Empty;

            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Password is required")]
            [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
            [System.ComponentModel.DataAnnotations.Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;
        }
    }
}