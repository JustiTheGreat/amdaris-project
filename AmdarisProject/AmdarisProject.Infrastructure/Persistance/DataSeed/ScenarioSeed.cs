using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Persistance.DataSeed
{
    internal class ScenarioSeed
    {
        public static async Task Seed(AmdarisProjectDBContext dbContext)
        {
            string test = "Test";

            GameFormat gameFormat = new()
            {
                Name = $"{test}{nameof(GameFormat)}",
                GameType = dbContext.Set<GameType>().First(),
                CompetitorType = CompetitorType.TEAM,
                TeamSize = 2,
                WinAt = 3,
                DurationInMinutes = null,
            };

            Competition competition = new TournamentCompetition()
            {
                Name = $"{test}{nameof(TournamentCompetition)}",
                Location = $"{test}Location",
                StartTime = DateTime.Now,
                Status = CompetitionStatus.ORGANIZING,
                GameFormat = gameFormat,
                BreakInMinutes = null,
                Competitors = [],
                Matches = [],
                StageLevel = 0
            };

            int numberOfPlayers = 8;

            List<Player> players = [];
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Player player = new()
                {
                    Name = $"{test}{nameof(Player)}{i + 1}",
                    Matches = [],
                    WonMatches = [],
                    Competitions = [],
                    TeamPlayers = [],
                    Points = [],
                    Teams = []
                };
                players.Add(player);
            }

            List<Team> teams = [];
            for (int i = 0; i < numberOfPlayers / 2; i++)
            {
                Team team = new()
                {
                    Name = $"{test}{nameof(Team)}{i + 1}",
                    Matches = [],
                    WonMatches = [],
                    Competitions = [],
                    TeamPlayers = [],
                    Players = []
                };
                teams.Add(team);
                team.Players.Add(players[2 * i]);
                team.Players.Add(players[2 * i + 1]);
                competition.Competitors.Add(team);
            }

            await dbContext.AddAsync(competition);
            await dbContext.SaveChangesAsync();

            await dbContext.TeamPlayers.ForEachAsync(teamPlayer => teamPlayer.IsActive = true);

            await dbContext.SaveChangesAsync();
        }
    }
}
