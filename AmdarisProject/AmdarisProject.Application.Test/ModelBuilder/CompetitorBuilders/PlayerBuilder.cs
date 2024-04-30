using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Test.ModelBuilder.CompetitorBuilders
{
    internal class PlayerBuilder : CompetitiorBuilder<Player, PlayerBuilder>
    {
        private PlayerBuilder(Player player) : base(player) { }

        public static PlayerBuilder CreateBasic()
            => new(new Player()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Matches = [],
                WonMatches = [],
                Competitions = [],
                Points = [],
                Teams = [],
                TeamPlayers = []
            });
    }
}
