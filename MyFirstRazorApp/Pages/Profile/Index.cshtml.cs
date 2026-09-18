using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MyFirstRazorApp.Pages.Profile
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;

        public void OnGet()
        {
            UserId = int.Parse(User.Claims.FirstOrDefault(s => s.Type == "UserId")?.Value ?? "0");
            UserName = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.Name)?.Value ?? "";
            UserEmail = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.Email)?.Value ?? "";
            UserRole = User.Claims.FirstOrDefault(s => s.Type == ClaimTypes.Role)?.Value ?? "";
        }
    }
}
