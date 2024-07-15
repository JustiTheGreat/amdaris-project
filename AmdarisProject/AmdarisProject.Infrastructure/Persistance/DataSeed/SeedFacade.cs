using AmdarisProject.Infrastructure.Options;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using Microsoft.AspNetCore.Identity;

namespace AmdarisProject.Infrastructure.Persistance.DataSeed
{
    public class SeedFacade
    {
        public static async Task SeedData(AmdarisProjectDBContext dbContext,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AdministratorData administratorData)
        {
            if (await dbContext.Database.EnsureCreatedAsync())
            {
                await GameTypeSeed.Seed(dbContext);
                await RoleSeed.Seed(roleManager);
                await AdministratorSeed.Seed(userManager, administratorData);
            }
        }
    }
}
