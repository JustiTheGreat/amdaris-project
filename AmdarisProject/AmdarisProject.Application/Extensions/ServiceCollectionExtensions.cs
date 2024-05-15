using AmdarisProject.Application.Common.Abstractions;
using AmdarisProject.Application.Common.Abstractions.RepositoryAbstractions;
using AmdarisProject.Application.Services;
using AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService;
using AmdarisProject.Application.Services.CompetitionMatchCreatorServices;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnlineBookShop.Application.Common.Behaviours;
using System.Reflection;

namespace OnlineBookShop.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddValidatorsFromAssembly(assembly);
            services
                .AddScoped<ICompetitionRankingService, CompetionRankingService>()
                .AddScoped<ICompetitionMatchCreatorFactoryService, CompetitionMatchCreatorFactoryService>()
                .AddScoped<IOneVsAllCompetitionMatchCreatorService, OneVsAllCompetitionMatchCreatorService>()
                .AddScoped<ITournamentCompetitionMatchCreatorService, TournamentCompetitionMatchCreatorService>()
                .AddScoped<IEndMatchService, EndMatchService>()
                .AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assembly))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }
}
