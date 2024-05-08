using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Services;
using AmdarisProject.Application.Services.CompetitionMatchCreatorFactoryService.MatchCreatorService;
using AmdarisProject.Application.Services.CompetitionMatchCreatorServices;
using AmdarisProject.Infrastructure;
using AmdarisProject.Infrastructure.Repositories;
using AmdarisProject.Presentation;
using AmdarisProject.Presentation.Middleware;
using AmdarisProject.Presentation.Options;
using MapsterMapper;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
string handlerAssemblyName = (string)builder.Configuration.GetValue(typeof(string), "HandlerAssembly")!;
Assembly handlerAssembly = Assembly.Load(handlerAssemblyName);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AmdarisProjectDBContext>();
builder.Services.AddScoped<ICompetitionRepository, CompetitionRepository>();
builder.Services.AddScoped<ICompetitorRepository, CompetitorRepository>();
builder.Services.AddScoped<IGameFormatRepository, GameFormatRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IPointRepository, PointRepository>();
builder.Services.AddScoped<ITeamPlayerRepository, TeamPlayerRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICompetitionMatchCreatorFactoryService, CompetitionMatchCreatorFactoryService>();
builder.Services.AddScoped<IOneVsAllCompetitionMatchCreatorService, OneVsAllCompetitionMatchCreatorService>();
builder.Services.AddScoped<ITournamentCompetitionMatchCreatorService, TournamentCompetitionMatchCreatorService>();
builder.Services.AddScoped<ICompetitionRankingService, CompetionRankingService>();
builder.Services.AddScoped<IEndMatchService, EndMatchService>();
builder.Services.AddScoped<IMapper>(sp => new Mapper(MapsterConfiguration.GetMapsterConfiguration()));
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(handlerAssembly));
builder.Services.AddScoped<IConnectionStrings>(sp =>
    (ConnectionStrings)builder.Configuration.GetRequiredSection(nameof(ConnectionStrings)).Get(typeof(ConnectionStrings))!);
//builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestCompletionTimeLoggingMiddleware>();
app.UseExceptionHandler("/Error");
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
