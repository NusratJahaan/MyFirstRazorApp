using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyFirstRazorApp.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyFirstRazorApp.Services
{
    public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser>
    {
        public AppUserClaimsPrincipalFactory(
            UserManager<AppUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // ✅ Add custom claims here
            var claims = await UserManager.GetClaimsAsync(user);
            identity.AddClaims(claims);

            return identity;
        }
    }
}