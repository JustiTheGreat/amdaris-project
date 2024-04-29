using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Services;
using AmdarisProject.Application.Services.CompetitionMatchCreatorServices;
using AmdarisProject.Infrastructure;
using AmdarisProject.Infrastructure.Repositories;
using AmdarisProject.Presentation;
using MapsterMapper;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<IMapper, Mapper>(sp => new Mapper(MapsterConfiguration.GetMapsterConfiguration()));
builder.Services.AddScoped<ICompetitionMatchCreatorFactoryService, CompetitionMatchCreatorFactoryService>();
builder.Services.AddScoped<ICompetitionRankingService, CompetionRankingService>();
builder.Services.AddScoped<IEndMatchService, EndMatchService>();
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(
    typeof(ICompetitionRepository).Assembly,
    typeof(ICompetitorRepository).Assembly,
    typeof(IMatchRepository).Assembly,
    typeof(IPointRepository).Assembly
));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
