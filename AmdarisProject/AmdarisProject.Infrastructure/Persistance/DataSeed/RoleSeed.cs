using AmdarisProject.Presentation;
using Microsoft.AspNetCore.Identity;

namespace AmdarisProject.Infrastructure.Persistance.DataSeed
{
    internal class RoleSeed
    {
        public static async Task Seed(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(nameof(UserRole.Administrator)));
                await roleManager.CreateAsync(new IdentityRole(nameof(UserRole.User)));
            }
        }
    }
}
