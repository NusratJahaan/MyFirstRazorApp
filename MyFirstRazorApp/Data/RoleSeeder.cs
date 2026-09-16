using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MyFirstRazorApp.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Coordinator", "Teacher", "Student" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}