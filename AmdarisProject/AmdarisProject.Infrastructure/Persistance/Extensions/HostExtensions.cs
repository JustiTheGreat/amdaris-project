using AmdarisProject.Infrastructure.Persistance.Contexts;
using AmdarisProject.Infrastructure.Persistance.DataSeed;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

                await SeedFacade.SeedData(context, userManager, roleManager);
            }
            catch (Exception)
            {
                //TODO program.cs class
                //var logger = services.GetRequiredService<ILogger<Program>>();
                //logger.LogError(ex, "An error occured during migration");
            }
        }
    }
}
