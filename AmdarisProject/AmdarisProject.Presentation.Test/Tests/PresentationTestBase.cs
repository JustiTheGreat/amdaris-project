using AmdarisProject.Application;
using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Application.Common.Models;
using AmdarisProject.Application.Services;
using AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService;
using AmdarisProject.Application.Services.CompetitionMatchCreatorServices;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Infrastructure.Persistance;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using AmdarisProject.Infrastructure.Persistance.Repositories;
using AmdarisProject.TestUtils;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AmdarisProject.Presentation.Test.Tests
{
    public abstract class PresentationTestBase<CONTROLLER> where CONTROLLER : class
    {
        protected readonly IMapper _mapper = AutoMapperConfiguration.GetMapper();
        protected readonly int _numberOfModelsInAList = 5;
        protected readonly PagedRequest _pagedRequest;

        protected AmdarisProjectDBContext _dbContext;
        protected CONTROLLER _controller;

        protected PresentationTestBase()
        {
            _pagedRequest = new PagedRequest()
            {
                PageIndex = 0,
                PageSize = _numberOfModelsInAList,
                ColumnNameForSorting = string.Empty,
                SortDirection = SortDirection.ASC,
            };
        }

        protected void Setup<RECORD, RETURN, HANDLER>()
            where HANDLER : class, IRequestHandler<RECORD, RETURN>
            where RECORD : IRequest<RETURN>
        {
            using AmdarisProjectDBContextBuilder builder = new();
            _dbContext = builder.GetContext();
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddMediatR(configuration =>
                    configuration.RegisterServicesFromAssemblies(typeof(HandlerAssemblyMarker).Assembly))
                .AddScoped<ICompetitionRankingService, CompetionRankingService>()
                .AddScoped<ICompetitionMatchCreatorFactoryService, CompetitionMatchCreatorFactoryService>()
                .AddScoped<IOneVsAllCompetitionMatchCreatorService, OneVsAllCompetitionMatchCreatorService>()
                .AddScoped<ITournamentCompetitionMatchCreatorService, TournamentCompetitionMatchCreatorService>()
                .AddScoped<IEndMatchService, EndMatchService>()
                .AddSingleton<IUnitOfWork>(GetUnitOfWork(_dbContext))
                .AddAutoMapper(typeof(AutoMapperProfileAssemblyMarker))
                .AddLogging()
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
