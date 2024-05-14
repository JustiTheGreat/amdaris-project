using AmdarisProject.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace AmdarisProject.Presentation.Extensions
{
    public static class ServiceCollectionAddIdentityService
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddScoped<IdentityService>()
                .AddIdentityCore<IdentityUser>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 5;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;

                })
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<AmdarisProjectDBContext>();
            return serviceCollection;
        }
    }
}
