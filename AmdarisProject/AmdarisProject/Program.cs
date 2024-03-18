using AmdarisProject.models;
using AmdarisProject.models.competition;
using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using System.Runtime.Intrinsics;

Game pingPong = new(3, GameType.PING_PONG, CompetitorType.PLAYER);
Game pingPongTeam = new(3, GameType.PING_PONG, CompetitorType.TWO_PLAYER_TEAM);
Game chess = new(1, GameType.CHESS, CompetitorType.PLAYER);
Game fifa = new(10, GameType.FIFA, CompetitorType.PLAYER);

Player player1 = new("Player1");
Player player2 = new("Player2");
Player player3 = new("Player3");
Player player4 = new("Player4");
Player player5 = new("Player5");
Player player6 = new("Player6");
Player player7 = new("Player7");
Player player8 = new("Player8");

TwoPlayerTeam team1 = new("Team1");
TwoPlayerTeam team2 = new("Team2");
TwoPlayerTeam team3 = new("Team3");
TwoPlayerTeam team4 = new("Team4");

team1.SetPlayerOne(player1);
team1.SetPlayerTwo(player2);
team2.SetPlayerOne(player3);
team2.SetPlayerTwo(player4);
team3.SetPlayerOne(player5);
team3.SetPlayerTwo(player6);
team4.SetPlayerOne(player7);
team4.SetPlayerTwo(player8);

Competition competition1 = new OneVSAllCompetition("c1", "l1", DateTime.Now, pingPong);
competition1.Register(player1);
competition1.Register(player2);
competition1.Register(player3);

Competition competition2 = new TournamentCompetition("c2", "l1", DateTime.Now, pingPong);
competition2.Register(player1);
competition2.Register(player3);
competition2.Register(player5);
competition2.Register(player7);

Competition competition3 = new TournamentCompetition("c3", "l1", DateTime.Now, pingPongTeam);
competition3.Register(team1);
competition3.Register(team2);
competition3.Register(team3);
competition3.Register(team4);

IEnumerable<TwoPlayerTeam> teams = [team1, team2, team3, team4];
IEnumerable<Player> players = [player1, player2, player3, player4, player5, player6, player7, player8];
var l1 = teams.Join(players, team => team.PlayerOne, player => player,
    (team, player) => new { TeamName = team.Name, PlayerName = player.Name })
    .ToList();
var l2 = teams.Join(players, team => team.PlayerTwo, player => player,
    (team, player) => new { TeamName = team.Name, PlayerName = player.Name })
    .ToList();
l1.Zip(l2, (e1, e2) => new { e1.TeamName, PlayerOneName = e1.PlayerName, PlayerTwoName = e2.PlayerName })
    .ToList().ForEach(e => Console.WriteLine($"{e.TeamName} -> {e.PlayerOneName}, {e.PlayerTwoName}")); ;

IEnumerable<Player> p1 = [player1, player3, player5, player7];
IEnumerable<Player> p2 = [player1, player2, player3, player4];
p1.Concat(p2).Select(e => e.Name).ToList().ForEach(e => Console.Write($"{e} "));
Console.WriteLine();
p1.Union(p2).Select(e => e.Name).ToList().ForEach(e => Console.Write($"{e} "));
Console.WriteLine();
p1.Intersect(p2).Select(e => e.Name).ToList().ForEach(e => Console.Write($"{e} "));
Console.WriteLine();
p1.Except(p2).Select(e => e.Name).ToList().ForEach(e => Console.Write($"{e} "));
Console.WriteLine();

IEnumerable<long> numbers = [45, 12, 63, 78, 5, 9, 21];
Console.WriteLine($"Count: {numbers.Count()}");
Console.WriteLine($"Min: {numbers.Min()}");
Console.WriteLine($"Max: {numbers.Max()}");
Console.WriteLine($"Sum: {numbers.Sum()}");
Console.WriteLine($"Average: {numbers.Average()}");
Console.WriteLine($"Product: {numbers.Aggregate((e1, e2) => e1 * e2)}");
Console.WriteLine($"Contains 9: {numbers.Contains(9)}");
Console.WriteLine($"Has elements: {numbers.Any()}");
Console.WriteLine($"All elements are bigger than 0: {numbers.All(e => e > 0)}");
IEnumerable<long> numbers2 = [12, 63, 78, 5, 9, 21, 45];
Console.WriteLine($"Equals sequence: {numbers.SequenceEqual(numbers2)}");
Console.WriteLine($"First element smaller than 10: {numbers.First(e => e < 10)}");
Console.WriteLine($"Last element smaller than 10: {numbers.Last(e => e < 10)}");
var s = Enumerable.Empty<int>();
Enumerable.Repeat(0, 10).ToList().ForEach(e => Console.Write($"{e} "));
Console.WriteLine();
Enumerable.Range(1, 10).ToList().ForEach(e => Console.Write($"{e} "));
Console.WriteLine();

//simulateCompetition(competition3);

void simulateCompetition(Competition competition)
{
    Console.WriteLine();
    competition.StopRegistrations();
    competition.Start();
    Console.WriteLine();

    while (competition.GetUnfinishedMatches().Any())
    {
        foreach (Match match in competition.GetUnfinishedMatches())
            SimulateMatch(match);
    }

    competition.End();
    Console.WriteLine($"The winner of competition {competition.Name} is competitor {competition.GetWinner().Name}");
    Console.WriteLine($"Matches played: {competition.Matches.Count()}");

    Console.WriteLine("Player ratings:");
    foreach (Competitor competitor in competition.Competitors)
        Console.WriteLine($"{competitor.Name}: {competitor.GetRating(competition.Game.Type)}");
}

void SimulateMatch(Match match)
{
    match.Start();
    while (match.GetPointsCompetitorOne() != match.Game.WinAt
        && match.GetPointsCompetitorTwo() != match.Game.WinAt)
    {
        Competitor competitor = new Random().Next(2) == 0 ? match.CompetitorOne : match.CompetitorTwo;
        SimulateAddPointsToCompetitor(competitor, match);
    }
    match.End();

    Console.WriteLine($"Winner: {match.GetWinner()?.Name ?? "DRAW"}");
    Console.WriteLine();
}

void SimulateAddPointsToCompetitor(Competitor competitor, Match match)
{
    Player player = competitor as Player
        ?? (new Random().Next(2) == 0 ? (competitor as TwoPlayerTeam)?.PlayerOne! : (competitor as TwoPlayerTeam)?.PlayerTwo!);
    player.AddPoints(match, 1);
}