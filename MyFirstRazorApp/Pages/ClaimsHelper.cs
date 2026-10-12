using System.Security.Claims;

namespace MyFirstRazorApp.Pages
{
    public static class ClaimsHelper
    {
        public static string GetRole(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
        }

        public static int? GetUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            return int.TryParse(userIdClaim, out int userId) ? userId : null;
        }
    }
}
