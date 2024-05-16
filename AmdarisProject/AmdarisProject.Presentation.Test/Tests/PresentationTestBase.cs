using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Application.Services;
using AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService;
using AmdarisProject.Application.Services.CompetitionMatchCreatorServices;
using AmdarisProject.Domain.Models;
using AmdarisProject.Infrastructure;
using AmdarisProject.Infrastructure.Persistance;
using AmdarisProject.Infrastructure.Persistance.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AmdarisProject.Presentation.Test.Tests
{
    public abstract class PresentationTestBase<CONTROLLER> where CONTROLLER : class
    {
        protected readonly int _numberOfModelsInAList = 5;

        protected AmdarisProjectDBContext _dbContext;
        protected CONTROLLER _controller;

        private static bool _mapsterWasConfigured = false;

        protected PresentationTestBase()
        {
        }

        protected void Setup<RECORD, RETURN, HANDLER>()
            where HANDLER : class, IRequestHandler<RECORD, RETURN>
            where RECORD : IRequest<RETURN>
        {
            using AmdarisProjectDBContextBuilder builder = new();
            _dbContext = builder.GetContext();
            IUnitOfWork unitOfWork = GetUnitOfWork(_dbContext);
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddMediatR(configuration =>
                   configuration.RegisterServicesFromAssemblies(Assembly.Load("AmdarisProject.Application")))//TODO assembly
                //.AddScoped<IMapper>(sp => new Mapper(MapsterConfiguration.GetMapsterConfiguration()))
                .AddLogging()
                .AddSingleton(sp => unitOfWork)
                .AddScoped<IEndMatchService, EndMatchService>()
                .AddScoped<ICompetitionMatchCreatorFactoryService, CompetitionMatchCreatorFactoryService>()
                .AddScoped<IOneVsAllCompetitionMatchCreatorService, OneVsAllCompetitionMatchCreatorService>()
                .AddScoped<ITournamentCompetitionMatchCreatorService, TournamentCompetitionMatchCreatorService>()
                .AddScoped<IRequestHandler<RECORD, RETURN>, HANDLER>()
                .AddScoped<CONTROLLER>()
                .BuildServiceProvider();
            _controller = serviceProvider.GetRequiredService<CONTROLLER>();
        }

        private static UnitOfWork GetUnitOfWork(AmdarisProjectDBContext dbContext)
            => new(dbContext,
                new CompetitionRepository(dbContext),
                new CompetitorRepository(dbContext),
                new GameFormatRepository(dbContext),
                new MatchRepository(dbContext),
                new PointRepository(dbContext),
                new TeamPlayerRepository(dbContext));

        protected void Detach(Model model) => _dbContext.Entry(model).State = EntityState.Detached;
    }
}
