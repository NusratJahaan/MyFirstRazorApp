using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyFirstRazorApp.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;

        public LoginModel(IUserService userService)
        {
            _userService = userService;
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
                // ✅ Validate user against DB
                var user = await _userService.ValidateUserAsync(Input.Email, Input.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return Page();
                }

                // ✅ Build claims: Id, UserName, Role
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // ✅ Sign in with cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToPage("/Index");
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
            [BindProperty]
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Email is required")]
            [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Please enter a valid email")]
            [System.ComponentModel.DataAnnotations.Display(Name = "Email Address")]
            public string Email { get; set; } = string.Empty;

            [BindProperty]
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Password is required")]
            [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
            [System.ComponentModel.DataAnnotations.Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;
        }
    }
}