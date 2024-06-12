using AmdarisProject.Domain.Models;
using AmdarisProject.Infrastructure.Persistance.Contexts;

namespace AmdarisProject.Infrastructure.Persistance.DataSeed
{
    internal class GameTypeSeed
    {
        public static async Task Seed(AmdarisProjectDBContext dbContext)
        {
            List<string> gameTypes = ["Ping pong", "Chess", "FIFA", "Football", "Basketball", "Tenis", "Hockey", "Rugby"];

            if (!dbContext.GameTypes.Any())
            {
                gameTypes.ForEach(gameType => dbContext.GameTypes.Add(new GameType() { Name = gameType }));
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
