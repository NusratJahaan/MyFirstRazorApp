using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyFirstRazorApp.Pages.Science
{
    [Authorize(Policy = "ScienceOnly")]  // ✅ Protected by policy
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}