using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Infrastructure.Persistance.Contexts;

namespace AmdarisProject.Infrastructure.Persistance.DataSeed
{
    internal class ScenarioSeed
    {
        public static async Task Seed(AmdarisProjectDBContext dbContext)
        {
            string test = "Test";

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
                team.Players.Add(players[2 * i]);
                team.Players.Add(players[2 * i + 1]);
                await dbContext.AddAsync(team);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
