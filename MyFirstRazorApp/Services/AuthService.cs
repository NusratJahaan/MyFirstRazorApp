using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MyFirstRazorApp.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services
{
    public class AuthService : IAuthService
    {
        public async Task<List<Claim>> SetupAuthClaims(SystemUser user, HttpContext context)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.IsPersistent, "false", "bool"),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.Id.ToString()),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return claims;
        }
    }
}