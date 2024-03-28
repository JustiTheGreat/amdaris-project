using AmdarisProject;
using AmdarisProject.handlers.competition;
using AmdarisProject.handlers.competitor;
using AmdarisProject.handlers.point;
using AmdarisProject.models;
using AmdarisProject.models.competition;
using AmdarisProject.models.competitor;
using AmdarisProject.repositories;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

ServiceProvider serviceProvider = new ServiceCollection()
    .AddSingleton<ICompetitionRepository, CompetitionRepository>()
    .AddSingleton<ICompetitorRepository, CompetitorRepository>()
    .AddSingleton<IMatchRepository, MatchRepository>()
    .AddSingleton<IPointRepository, PointRepository>()
    .AddSingleton<IStageRepository, StageRepository>()
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

GameRules pingPong = new(3, null, null, GameType.PING_PONG);
GameRules pingPongTeam = new(3, null, null, GameType.PING_PONG, myTeamSize);
GameRules pingPongTeamTimed = new(null, 5, 1, GameType.PING_PONG, myTeamSize);
GameRules chess = new(1, null, null, GameType.CHESS);
GameRules fifa = new(10, null, null, GameType.FIFA);

ulong player1Id = mediator.Send(new CreatePlayer("Player1")).Result.Id;
ulong player2Id = mediator.Send(new CreatePlayer("Player2")).Result.Id;
ulong player3Id = mediator.Send(new CreatePlayer("Player3")).Result.Id;
ulong player4Id = mediator.Send(new CreatePlayer("Player4")).Result.Id;
ulong player5Id = mediator.Send(new CreatePlayer("Player5")).Result.Id;
ulong player6Id = mediator.Send(new CreatePlayer("Player6")).Result.Id;
ulong player7Id = mediator.Send(new CreatePlayer("Player7")).Result.Id;
ulong player8Id = mediator.Send(new CreatePlayer("Player8")).Result.Id;

ulong team1Id = mediator.Send(new CreateTeam("Team1", myTeamSize)).Result.Id;
ulong team2Id = mediator.Send(new CreateTeam("Team2", myTeamSize)).Result.Id;
ulong team3Id = mediator.Send(new CreateTeam("Team3", myTeamSize)).Result.Id;
ulong team4Id = mediator.Send(new CreateTeam("Team4", myTeamSize)).Result.Id;

await mediator.Send(new AddPlayerToTeam(player1Id, team1Id));
await mediator.Send(new AddPlayerToTeam(player2Id, team1Id));
await mediator.Send(new AddPlayerToTeam(player3Id, team2Id));
await mediator.Send(new AddPlayerToTeam(player4Id, team2Id));
await mediator.Send(new AddPlayerToTeam(player5Id, team3Id));
await mediator.Send(new AddPlayerToTeam(player6Id, team3Id));
await mediator.Send(new AddPlayerToTeam(player7Id, team4Id));
await mediator.Send(new AddPlayerToTeam(player8Id, team4Id));

string location = "Amdaris";

ulong competition1Id = mediator.Send(new CreateOneVSAllCompetition("c1", location, DateTime.Now, pingPong)).Result.Id;
ulong competition2Id = mediator.Send(new CreateOneVSAllCompetition("c2", location, DateTime.Now, pingPongTeam)).Result.Id;
ulong competition3Id = mediator.Send(new CreateTournamentCompetition("c3", location, DateTime.Now, pingPong)).Result.Id;
ulong competition4Id = mediator.Send(new CreateTournamentCompetition("c4", location, DateTime.Now, pingPongTeam)).Result.Id;
ulong competition5Id = mediator.Send(new CreateTournamentCompetition("c5", location, DateTime.Now, pingPongTeamTimed)).Result.Id;

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

ulong competitionToTestId = competition1Id;
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