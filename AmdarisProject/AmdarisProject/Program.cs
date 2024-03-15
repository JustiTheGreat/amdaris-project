using AmdarisProject.models;
using AmdarisProject.models.competition;
using AmdarisProject.models.competitor;
using AmdarisProject.repositories;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.Exceptions;

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

GenericRepository<Player> playerRepository = new();
playerRepository.Add(player1);
playerRepository.Add(player2);
playerRepository.Add(player3);
playerRepository.Add(player4);
printPlayers(playerRepository);
playerRepository.Add(player5);
playerRepository.Add(player6);
playerRepository.Add(player7);
playerRepository.Add(player8);
printPlayers(playerRepository);
playerRepository.Delete(player2.Id);
playerRepository.Delete(player6.Id);
playerRepository.Delete(player4.Id);
playerRepository.Delete(player8.Id);
printPlayers(playerRepository);
Console.WriteLine(playerRepository.GetById(player3.Id).Name);
void printPlayers(GenericRepository<Player> playersRepository)
{
    foreach (Player player in playersRepository.GetAll())
        Console.Write($"{player.Name}, ");
    Console.WriteLine();
}

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
//competition.Register(player4);
competition.StopRegistrations();
competition.Start();


while (competition.GetUnfinishedMatches().Any())
{
    foreach (Match match in competition.GetUnfinishedMatches())
        SimulateMatch(match);
}

competition.End();
Console.WriteLine($"The winner of competition {competition.Name} is competitor {competition.GetWinner().Name}");
Console.WriteLine($"Matches played: {competition.Matches.Count()}");

foreach (Competitor competitor in competition.Competitors)
{
    Console.WriteLine(competitor.GetRating(competition.Game.Type));
}

void SimulateMatch(Match match)
{
    match.Start();
    while (match.GetPointsCompetitorOne() != match.Game.WinAt && match.GetPointsCompetitorTwo() != match.Game.WinAt)
    {
        int pointGoesToCompetitor = new Random().Next(2);
        Competitor competitor = pointGoesToCompetitor == 0 ? match.CompetitorOne : match.CompetitorTwo;
        SimulateAddPointsToCompetitor(competitor, match);
    }
    match.End();

    try
    {
        Competitor? winner = match.GetWinner();
        if (winner is null)
            Console.WriteLine($"Winner: CANCELED");
        else
            Console.WriteLine($"Winner: {winner.Name}");
        Console.WriteLine();
    }
    catch (DrawMatchResultException)
    {
        Console.WriteLine($"Winner: DRAW");
    }
}

void SimulateAddPointsToCompetitor(Competitor competitor, Match match)
{
    Player? player = competitor as Player;
    if (player is null)
    {
        int pointGoesToPlayer = new Random().Next(2);
        player = pointGoesToPlayer == 0 ? (competitor as TwoPlayerTeam)!.PlayerOne : (competitor as TwoPlayerTeam)!.PlayerTwo;
    }
    player!.AddPoints(match, 1);
}