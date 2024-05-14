using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Services;
using AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService;
using AmdarisProject.Application.Services.CompetitionMatchCreatorServices;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Infrastructure;
using AmdarisProject.Infrastructure.Repositories;
using AmdarisProject.Presentation;
using AmdarisProject.Presentation.Extensions;
using AmdarisProject.Presentation.Middleware;
using AmdarisProject.Presentation.Options;
using MapsterMapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
string handlerAssemblyName = (string)builder.Configuration.GetValue(typeof(string), "HandlerAssembly")!;
Assembly handlerAssembly = Assembly.Load(handlerAssemblyName);

JwtSettings jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
    ?? throw new AmdarisProjectException("Missing JWT settings!");

builder.Services.AddControllers();
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerWithAuthorization()
    .AddDbContext<AmdarisProjectDBContext>()
    .AddScoped<ICompetitionRepository, CompetitionRepository>()
    .AddScoped<ICompetitorRepository, CompetitorRepository>()
    .AddScoped<IGameFormatRepository, GameFormatRepository>()
    .AddScoped<IMatchRepository, MatchRepository>()
    .AddScoped<IPointRepository, PointRepository>()
    .AddScoped<ITeamPlayerRepository, TeamPlayerRepository>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped<ICompetitionRankingService, CompetionRankingService>()
    .AddScoped<ICompetitionMatchCreatorFactoryService, CompetitionMatchCreatorFactoryService>()
    .AddScoped<IOneVsAllCompetitionMatchCreatorService, OneVsAllCompetitionMatchCreatorService>()
    .AddScoped<ITournamentCompetitionMatchCreatorService, TournamentCompetitionMatchCreatorService>()
    .AddScoped<IEndMatchService, EndMatchService>()
    .AddScoped<IMapper>(sp => new Mapper(MapsterConfiguration.GetMapsterConfiguration()))
    .AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(handlerAssembly))
    .Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)))
    .Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)))
    .AddAuthenticationService(jwtSettings)
    .AddIdentityService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestCompletionTimeLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
