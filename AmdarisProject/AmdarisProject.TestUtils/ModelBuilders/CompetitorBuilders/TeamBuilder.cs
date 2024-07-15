using AmdarisProject.Domain.Models;
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
            TeamPlayers = [],
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
            TeamPlayer teamPlayer = APBuilder.CreateBasicTeamPlayer().SetTeam(_model).SetPlayer(player).SetIsActive(false).Get();
            _model.Players.Add(player);
            _model.TeamPlayers.Add(teamPlayer);
            player.Teams.Add(_model);
            player.TeamPlayers.Add(teamPlayer);
            return this;
        }
    }
}
