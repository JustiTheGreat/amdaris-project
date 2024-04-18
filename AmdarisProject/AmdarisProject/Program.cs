using AmdarisProject.Application;
using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitionResponseDTOs;
using AmdarisProject.Application.Dtos.ResponseDTOs.CompetitorResponseDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Handlers.MatchHandlers;
using AmdarisProject.Application.Services;
using AmdarisProject.Application.Services.CompetitionMatchCreatorServices;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.handlers.competition;
using AmdarisProject.handlers.point;
using AmdarisProject.Infrastructure;
using AmdarisProject.Infrastructure.Repositories;
using AmdarisProject.Presentation;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

IServiceProvider serviceProvider = new ServiceCollection()
    .AddDbContext<AmdarisProjectDBContext>()
    .AddScoped<ICompetitionRepository, CompetitionRepository>()
    .AddScoped<ICompetitorRepository, CompetitorRepository>()
    .AddScoped<IMatchRepository, MatchRepository>()
    .AddScoped<IPointRepository, PointRepository>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped<IMapper, Mapper>(sp => new Mapper(MapsterConfiguration.GetMapsterConfiguration()))
    .AddScoped<ICreateCompetitionMatchesService, CreateCompetitionMatchesService>()
    .AddScoped<ICompetitionMatchCreatorFactoryService, CompetitionMatchCreatorFactoryService>()
    .AddScoped<ICompetitionRankingService, CompetionRankingService>()
    .AddScoped<IEndMatchService, EndMatchService>()
    .AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(
        typeof(ICompetitionRepository).Assembly,
        typeof(ICompetitorRepository).Assembly,
        typeof(IMatchRepository).Assembly,
        typeof(IPointRepository).Assembly
    ))
    .BuildServiceProvider();

IMediator mediator = serviceProvider.GetRequiredService<IMediator>();
IUnitOfWork unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

ushort myTeamSize = 2;

GameRules pingPong = new(3, null, null, GameType.PING_PONG, CompetitorType.PLAYER, null);
GameRules pingPongTeam = new(3, null, null, GameType.PING_PONG, CompetitorType.TEAM, myTeamSize);
GameRules pingPongTeamTimed = new(null, 5, 1, GameType.PING_PONG, CompetitorType.TEAM, myTeamSize);

Guid player1Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player1" })).Result.Id;
Guid player2Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player2" })).Result.Id;
Guid player3Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player3" })).Result.Id;
Guid player4Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player4" })).Result.Id;
Guid player5Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player5" })).Result.Id;
Guid player6Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player6" })).Result.Id;
Guid player7Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player7" })).Result.Id;
Guid player8Id = mediator.Send(new CreateCompetitor(new PlayerCreateDTO() { Name = "Player8" })).Result.Id;

Guid team1Id = mediator.Send(new CreateCompetitor(new TeamCreateDTO() { Name = "Team1", TeamSize = myTeamSize })).Result.Id;
Guid team2Id = mediator.Send(new CreateCompetitor(new TeamCreateDTO() { Name = "Team2", TeamSize = myTeamSize })).Result.Id;
Guid team3Id = mediator.Send(new CreateCompetitor(new TeamCreateDTO() { Name = "Team3", TeamSize = myTeamSize })).Result.Id;
Guid team4Id = mediator.Send(new CreateCompetitor(new TeamCreateDTO() { Name = "Team4", TeamSize = myTeamSize })).Result.Id;

await mediator.Send(new AddPlayerToTeam(player1Id, team1Id));
await mediator.Send(new AddPlayerToTeam(player2Id, team1Id));
await mediator.Send(new AddPlayerToTeam(player3Id, team2Id));
await mediator.Send(new AddPlayerToTeam(player4Id, team2Id));
await mediator.Send(new AddPlayerToTeam(player5Id, team3Id));
await mediator.Send(new AddPlayerToTeam(player6Id, team3Id));
await mediator.Send(new AddPlayerToTeam(player7Id, team4Id));
await mediator.Send(new AddPlayerToTeam(player8Id, team4Id));

string location = "Amdaris";

OneVSAllCompetitionCreateDTO createOVAC(string name, GameRules gameRules)
{
    return new OneVSAllCompetitionCreateDTO()
    {
        Name = name,
        Location = location,
        StartTime = DateTime.Now,
        Status = CompetitionStatus.ORGANIZING,
        WinAt = gameRules.WinAt,
        DurationInSeconds = gameRules.DurationInSeconds,
        BreakInSeconds = gameRules.BreakInSeconds,
        GameType = gameRules.Type,
        CompetitorType = gameRules.CompetitorType,
        TeamSize = gameRules.TeamSize,
    };
}
CompetitionCreateDTO createTC(string name, GameRules gameRules)
{
    return new TournamentCompetitionCreateDTO()
    {
        Name = name,
        Location = location,
        StartTime = DateTime.Now,
        Status = CompetitionStatus.ORGANIZING,
        WinAt = gameRules.WinAt,
        DurationInSeconds = gameRules.DurationInSeconds,
        BreakInSeconds = gameRules.BreakInSeconds,
        GameType = gameRules.Type,
        CompetitorType = gameRules.CompetitorType,
        TeamSize = gameRules.TeamSize,
    };
}

Guid competition1Id = mediator.Send(new CreateCompetition(createOVAC("c1", pingPong))).Result.Id;
Guid competition2Id = mediator.Send(new CreateCompetition(createOVAC("c2", pingPongTeam))).Result.Id;
Guid competition3Id = mediator.Send(new CreateCompetition(createTC("c3", pingPong))).Result.Id;
Guid competition4Id = mediator.Send(new CreateCompetition(createTC("c4", pingPongTeam))).Result.Id;
Guid competition5Id = mediator.Send(new CreateCompetition(createTC("c5", pingPongTeamTimed))).Result.Id;

await mediator.Send(new AddCompetitorToCompetition(player1Id, competition1Id));
await mediator.Send(new AddCompetitorToCompetition(player2Id, competition1Id));
await mediator.Send(new AddCompetitorToCompetition(player3Id, competition1Id));

await mediator.Send(new AddCompetitorToCompetition(team1Id, competition2Id));
await mediator.Send(new AddCompetitorToCompetition(team2Id, competition2Id));
await mediator.Send(new AddCompetitorToCompetition(team3Id, competition2Id));

await mediator.Send(new AddCompetitorToCompetition(player1Id, competition3Id));
await mediator.Send(new AddCompetitorToCompetition(player2Id, competition3Id));
await mediator.Send(new AddCompetitorToCompetition(player3Id, competition3Id));
await mediator.Send(new AddCompetitorToCompetition(player4Id, competition3Id));

await mediator.Send(new AddCompetitorToCompetition(team1Id, competition4Id));
await mediator.Send(new AddCompetitorToCompetition(team2Id, competition4Id));
await mediator.Send(new AddCompetitorToCompetition(team3Id, competition4Id));
await mediator.Send(new AddCompetitorToCompetition(team4Id, competition4Id));

await mediator.Send(new AddCompetitorToCompetition(team1Id, competition5Id));
await mediator.Send(new AddCompetitorToCompetition(team2Id, competition5Id));
await mediator.Send(new AddCompetitorToCompetition(team3Id, competition5Id));
await mediator.Send(new AddCompetitorToCompetition(team4Id, competition5Id));

Guid competitionToTestId = competition1Id;

await simulateCompetition(competitionToTestId);

async Task simulateCompetition(Guid competitionId)
{
    Console.WriteLine();
    await mediator.Send(new StopCompetitionRegistration(competitionId));
    await mediator.Send(new StartCompetition(competitionId));
    Console.WriteLine();

    while ((await unitOfWork.MatchRepository.GetUnfinishedByCompetition(competitionId)).Any())
    {
        foreach (Match match in await unitOfWork.MatchRepository.GetUnfinishedByCompetition(competitionId))
            await SimulateMatch(match.Id, competitionId);
    }

    await mediator.Send(new EndCompetition(competitionId));

    IEnumerable<RankingItemDTO> ranking = await mediator.Send(new GetCompetitionRanking(competitionId));
    CompetitionResponseDTO competitionResponseDTO = await mediator.Send(new GetCompetitionById(competitionId));

    Console.WriteLine($"The winner of competition {competitionResponseDTO.Name} is competitor {ranking.ElementAt(0).CompetitorName}!");
    Console.WriteLine($"Matches played: {competitionResponseDTO.Matches.Count}");
    competitionResponseDTO.Matches
        .Select(id => mediator.Send(new GetMatchById(id)).Result)
        .ToList()
        .ForEach(match => Console.WriteLine($"{match.StartTime}-{match.EndTime}: " +
        $"{mediator.Send(new GetCompetitorById(match.CompetitorOne)).Result.Name}={match.CompetitorOnePoints}-" +
        $"{mediator.Send(new GetCompetitorById(match.CompetitorTwo)).Result.Name}={match.CompetitorTwoPoints}"));

    Console.WriteLine();

    Console.WriteLine("Player ratings:");
    competitionResponseDTO.Competitors
        .Select(id => new
        {
            Name = mediator.Send(new GetCompetitorById(id)).Result.Name,
            Rating = mediator.Send(new GetCompetitorWinRatingForGameType(id, competitionResponseDTO.GameType)).Result
        })
        .ToList()
        .ForEach(o => Console.WriteLine($"{o.Name}: {o.Rating}"));
}

async Task SimulateMatch(Guid matchId, Guid competitionId)
{
    await mediator.Send(new StartMatch(matchId));
    CompetitionResponseDTO competition = await mediator.Send(new GetCompetitionById(competitionId));

    while (mediator.Send(new GetMatchById(matchId)).Result.Status is not MatchStatus.FINISHED)
    {
        MatchResponseDTO match = await mediator.Send(new GetMatchById(matchId));
        Guid? scorer = null;

        if (competition.CompetitorType is CompetitorType.PLAYER)
        {
            scorer = new Random().Next(2) == 0 ? match.CompetitorOne : match.CompetitorTwo;
        }
        else if (competition.CompetitorType is CompetitorType.TEAM)
        {
            CompetitorResponseDTO competitorOne = await mediator.Send(new GetCompetitorById(match.CompetitorOne));
            CompetitorResponseDTO competitorTwo = await mediator.Send(new GetCompetitorById(match.CompetitorTwo));
            int random = new Random().Next((int)(2 * competition.TeamSize)!);
            scorer = random < 4 ? ((TeamResponseDTO)competitorOne).Players[random % 4]
                : ((TeamResponseDTO)competitorTwo).Players[(random - 4) % 4];
        }

        await mediator.Send(new AddValueToPointValue((Guid)scorer!, match.Id, 1));
    }

    MatchResponseDTO match2 = await mediator.Send(new GetMatchById(matchId));
    CompetitorResponseDTO winner = await mediator.Send(new GetCompetitorById((Guid)match2.Winner!));
    Console.WriteLine($"Winner: {winner.Name}");
    Console.WriteLine();
}