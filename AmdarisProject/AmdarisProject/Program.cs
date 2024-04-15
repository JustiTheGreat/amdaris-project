using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs;
using AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs;
using AmdarisProject.Application.Handlers.CompetitionHandlers;
using AmdarisProject.Application.Handlers.CompetitorHandlers;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
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
    .AddScoped<IStageRepository, StageRepository>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped<IMapper, Mapper>(sp => new Mapper(MapsterConfiguration.GetMapsterConfiguration()))
    .AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(
        typeof(ICompetitionRepository).Assembly,
        typeof(ICompetitorRepository).Assembly,
        typeof(IMatchRepository).Assembly,
        typeof(IPointRepository).Assembly,
        typeof(IStageRepository).Assembly
    ))
    .BuildServiceProvider();

IMediator mediator = serviceProvider.GetRequiredService<IMediator>();

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
//AmdarisProjectTimer.AddCompetitionToSupervise(competitionToTest);
//AmdarisProjectTimer.SetTimer();

//simulateCompetition(competitionToTest);

//Console.ReadLine();

//async void simulateCompetition(ulong competitionId)
//{
//    Console.WriteLine();
//    await mediator.Send(new StopCompetitionRegistration(competitionId));
//    await mediator.Send(new StartCompetition(competitionId));
//    Console.WriteLine();

//    while (competition.GetUnfinishedMatches().Any())
//    {
//        foreach (Match match in competition.GetUnfinishedMatches())
//            SimulateMatch(match);
//    }

//    //while (competition.GetUnfinishedMatches().Any()) ;
//    await mediator.Send(new EndCompetition(competitionId));

//    Console.WriteLine($"The winner of competition {competition.Name} is competitor {competition.GetWinner().Name}!");
//    Console.WriteLine($"Matches played: {competition.Matches.Count}");
//    competitionToTest.Matches.ForEach(match =>
//        Console.WriteLine($"{match.StartTime}-{match.EndTime}; {match.GetPointsCompetitorOne()}-{match.GetPointsCompetitorTwo()}"));

//    Console.WriteLine();

//    Console.WriteLine("Player ratings:");
//    foreach (Competitor competitor in competition.Competitors)
//        Console.WriteLine($"{competitor.Name}: {competitor.GetRating(competition.GameRules.Type)}");
//}

//void SimulateMatch(Match match)
//{
//    match.Start();
//    while (match.Status is not MatchStatus.FINISHED)
//    {
//        Competitor competitor = new Random().Next(2) == 0 ? match.CompetitorOne : match.CompetitorTwo;
//        SimulateAddPointsToCompetitor(competitor, match);
//    }

//    Console.WriteLine($"Winner: {match.GetWinner()?.Name ?? "DRAW"}");
//    Console.WriteLine();
//}

//void SimulateAddPointsToCompetitor(Competitor competitor, Match match)
//{
//    Player player = competitor as Player
//        ?? (competitor as Team)?.Players.ElementAt(new Random().Next(myTeamSize))!;
//    mediator.Send(new AddPointsToPoint(player));
//    player.AddPoints(match, 1);
//}