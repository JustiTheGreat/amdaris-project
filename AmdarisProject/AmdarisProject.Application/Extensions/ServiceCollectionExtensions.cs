using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Application.Behaviours;
using AmdarisProject.Application.Services;
using AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService;
using AmdarisProject.Application.Services.CompetitionMatchCreatorServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AmdarisProject.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var handlerAssembly = typeof(HandlerAssemblyMarker).Assembly;

            services
                .AddScoped<ICompetitionRankingService, CompetionRankingService>()
                .AddScoped<ICompetitionMatchCreatorFactoryService, CompetitionMatchCreatorFactoryService>()
                .AddScoped<IOneVsAllCompetitionMatchCreatorService, OneVsAllCompetitionMatchCreatorService>()
                .AddScoped<ITournamentCompetitionMatchCreatorService, TournamentCompetitionMatchCreatorService>()
                .AddScoped<IEndMatchService, EndMatchService>()
                .AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(handlerAssembly))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }
}
