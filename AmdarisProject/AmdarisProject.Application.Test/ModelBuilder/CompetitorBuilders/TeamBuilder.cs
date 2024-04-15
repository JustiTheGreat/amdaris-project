using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Test.ModelBuilder.CompetitorBuilders
{
    internal class TeamBuilder : CompetitiorBuilder<Team, TeamBuilder>
    {
        private TeamBuilder(Team team) : base(team) { }

        public static TeamBuilder CreateBasic()
            => new(new Team()
            {
                Id = ++_instances,
                Name = "Test",
                TeamSize = 2,
            });

        public TeamBuilder SetTeamSize(ushort teamSize)
        {
            _model.TeamSize = teamSize;
            return this;
        }

        public TeamBuilder AddPlayer(Player player)
        {
            _model.Players.Add(player);
            return this;
        }

        public TeamBuilder FillTeamWithPlayers()
        {
            for (int i = 0; i < _model.TeamSize; i++)
                AddPlayer(Builders.CreateBasicPlayer().Get());

            return this;
        }
    }
}
