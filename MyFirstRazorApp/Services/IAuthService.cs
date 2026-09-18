using MyFirstRazorApp.Models;
using System.Security.Claims;

namespace MyFirstRazorApp.Services
{
    public interface IAuthService
    {
        Task<List<Claim>> SetupAuthClaims(SystemUser user, HttpContext context);
    }
}