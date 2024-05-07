using AmdarisProject.Application.Test.ModelBuilder.ModelBuilder;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Test.ModelBuilder
{
    internal class TeamPlayerBuilder : ModelBuilder<TeamPlayer, TeamPlayerBuilder>
    {
        private TeamPlayerBuilder(TeamPlayer teamPlayer) : base(teamPlayer) { }

        public static TeamPlayerBuilder CreateBasic()
            => new(new TeamPlayer()
            {
                Id = Guid.NewGuid(),
                Team = Builders.CreateBasicTeam().Get(),
                Player = Builders.CreateBasicPlayer().Get(),
                IsActive = true
            });
    }
}
