using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Test.ModelBuilder.CompetitorBuilders
{
    internal class TeamBuilder : CompetitiorBuilder<Team, TeamBuilder>
    {
        private TeamBuilder(Team team) : base(team) { }

        public static TeamBuilder CreateBasic()
            => new(new Team()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Matches = [],
                WonMatches = [],
                Competitions = [],
                Players = [],
                TeamPlayers = []
            });

        public TeamBuilder AddPlayer(Player player)
        {
            _model.Players.Add(player);
            return this;
        }
    }
}
