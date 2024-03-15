using AmdarisProject.models;
using AmdarisProject.models.competition;
using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;

Game pingPong = new(3, GameType.PING_PONG, CompetitorType.PLAYER);
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

Competition competition = new OneVSAllCompetition("c1", "l1", DateTime.Now, pingPong);
competition.Register(player1);
competition.Register(player2);
competition.Register(player3);
competition.StopRegistrations();

Console.WriteLine();
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