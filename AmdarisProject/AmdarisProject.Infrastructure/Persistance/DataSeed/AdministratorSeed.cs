using AmdarisProject.Infrastructure.Options;
using AmdarisProject.Presentation;
using Microsoft.AspNetCore.Identity;

namespace AmdarisProject.Infrastructure.Persistance.DataSeed
{
    internal class AdministratorSeed
    {
        public static async Task Seed(UserManager<IdentityUser> userManager, AdministratorData administratorData)
        {
            if (!userManager.Users.Any())
            {
                var administrator = new IdentityUser()
                {
                    UserName = administratorData.Username,
                    Email = administratorData.Email,
                };

                await userManager.CreateAsync(administrator, administratorData.Password);

                string roleName = nameof(UserRole.Administrator);
                await userManager.AddToRoleAsync(administrator, roleName);
            }
        }
    }
}
