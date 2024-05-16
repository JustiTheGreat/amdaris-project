using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Infrastructure.Identity;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using AmdarisProject.Infrastructure.Persistance.Repositories;
using AmdarisProject.Presentation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AmdarisProject.Infrastructure.Persistance.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services
                .AddDbContext<AmdarisProjectDBContext>()
                .AddScoped<ICompetitionRepository, CompetitionRepository>()
                .AddScoped<ICompetitorRepository, CompetitorRepository>()
                .AddScoped<IGameFormatRepository, GameFormatRepository>()
                .AddScoped<IMatchRepository, MatchRepository>()
                .AddScoped<IPointRepository, PointRepository>()
                .AddScoped<ITeamPlayerRepository, TeamPlayerRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddTransient<ITokenService, TokenService>()
                .AddTransient<IAuthenticationService, AuthenticationService>()
                .AddIdentityCore<IdentityUser>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;

                })
                .AddRoles<IdentityRole>()
                .AddSignInManager()
                .AddEntityFrameworkStores<AmdarisProjectDBContext>();
            return services;
        }
    }
}
