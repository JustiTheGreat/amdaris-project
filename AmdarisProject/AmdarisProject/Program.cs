using AmdarisProject.models;
using AmdarisProject.models.competition;
using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;

int myTeamSize = 2;

GameRules pingPong = new(3, null, GameType.PING_PONG);
GameRules pingPongTeam = new(3, null, GameType.PING_PONG, myTeamSize);
GameRules chess = new(1, null, GameType.CHESS);
GameRules fifa = new(10, null, GameType.FIFA);

Player player1 = new("Player1");
Player player2 = new("Player2");
Player player3 = new("Player3");
Player player4 = new("Player4");
Player player5 = new("Player5");
Player player6 = new("Player6");
Player player7 = new("Player7");
Player player8 = new("Player8");

Team team1 = new("Team1", myTeamSize);
Team team2 = new("Team2", myTeamSize);
Team team3 = new("Team3", myTeamSize);
Team team4 = new("Team4", myTeamSize);

team1.AddPlayer(player1);
team1.AddPlayer(player2);
team2.AddPlayer(player3);
team2.AddPlayer(player4);
team3.AddPlayer(player5);
team3.AddPlayer(player6);
team4.AddPlayer(player7);
team4.AddPlayer(player8);

string location = "Amdaris";

Competition competition1 = new OneVSAllCompetition("c1", location, DateTime.Now, pingPong);
competition1.Register(player1);
competition1.Register(player2);
competition1.Register(player3);

Competition competition2 = new OneVSAllCompetition("c2", location, DateTime.Now, pingPongTeam);
competition2.Register(player1);
competition2.Register(player2);
competition2.Register(player3);

Competition competition3 = new TournamentCompetition("c3", location, DateTime.Now, pingPong);
competition3.Register(player1);
competition3.Register(player2);
competition3.Register(player3);
competition3.Register(player4);

Competition competition4 = new TournamentCompetition("c4", location, DateTime.Now, pingPongTeam);
competition4.Register(team1);
competition4.Register(team2);
competition4.Register(team3);
competition4.Register(team4);

simulateCompetition(competition4);

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
        Console.WriteLine($"{competitor.Name}: {competitor.GetRating(competition.GameRules.Type)}");
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
        ?? (competitor as Team)?.Players.ElementAt(new Random().Next(myTeamSize))!;
    player.AddPoints(match, 1);
}