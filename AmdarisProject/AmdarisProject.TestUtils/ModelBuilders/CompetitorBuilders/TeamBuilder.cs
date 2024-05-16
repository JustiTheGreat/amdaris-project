using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.TestUtils.ModelBuilders.CompetitorBuilders
{
    public class TeamBuilder : CompetitiorBuilder<Team, TeamBuilder>
    {
        public TeamBuilder() : base(new Team()
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Matches = [],
            WonMatches = [],
            Competitions = [],
            Players = []
        })
        { }

        public override TeamBuilder Clone()
            => new TeamBuilder()
            .SetId(_model.Id)
            .SetName(_model.Name)
            .SetMatches(_model.Matches)
            .SetWonMatches(_model.WonMatches)
            .SetCompetitions(_model.Competitions);

        public TeamBuilder SetPlayers(List<Player> players)
        {
            _model.Players = players;
            return this;
        }

        public TeamBuilder AddPlayer(Player player)
        {
            _model.Players.Add(player);
            player.Teams.Add(_model);
            return this;
        }
    }
}
