using AmdarisProject.Infrastructure.Options;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using AmdarisProject.Infrastructure.Persistance.DataSeed;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AmdarisProject.Infrastructure.Persistance.Extensions
{
    public static class HostExtensions
    {
        public static async Task SeedData(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AmdarisProjectDBContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var administratorData = services.GetService<IOptions<AdministratorData>>()!.Value;

                await SeedFacade.SeedData(context, userManager, roleManager, administratorData);
            }
            catch (Exception)
            {
                //TODO program.cs class
                //var logger = services.GetRequiredService<ILogger<Program>>();
                //logger.LogError(ex, "An error occured during migration");
                throw;
            }
        }
    }
}
