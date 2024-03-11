using AmdarisProject.models;
using AmdarisProject.models.competition;
using AmdarisProject.models.competitor;
using AmdarisProject.utils;

Player player1 = new("p1");
Player player2 = new("p2");
Player player3 = new("p3");
Player player4 = new("p4");
Player player5 = new("p5");
Player player6 = new("p6");
Player player7 = new("p7");
Player player8 = new("p8");

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

Competition competition = new OneVSAllCompetition("c1", "l1", DateTime.Now, GameType.PING_PONG, CompetitorType.PLAYER);
competition.Register(player1);
competition.Register(player2);
competition.Register(player3);
competition.Register(player4);
competition.StopRegistrations();

Match match1 = new Match(competition.Location, DateTime.Now, competition.GameType, player1, player2);
Match match2 = new Match(competition.Location, DateTime.Now, competition.GameType, player1, player3);
Match match3 = new Match(competition.Location, DateTime.Now, competition.GameType, player1, player4);
Match match4 = new Match(competition.Location, DateTime.Now, competition.GameType, player2, player3);
Match match5 = new Match(competition.Location, DateTime.Now, competition.GameType, player2, player4);
Match match6 = new Match(competition.Location, DateTime.Now, competition.GameType, player3, player4);

match1.Start();
player1.AddPoints(match1, 1);
player2.AddPoints(match2, 1);
player1.AddPoints(match1, 1);
player1.AddPoints(match1, 1);
