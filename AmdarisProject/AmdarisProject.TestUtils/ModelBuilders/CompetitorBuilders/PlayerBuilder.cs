using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.TestUtils.ModelBuilders.CompetitorBuilders
{
    public class PlayerBuilder : CompetitiorBuilder<Player, PlayerBuilder>
    {
        public PlayerBuilder() : base(new Player()
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Matches = [],
            WonMatches = [],
            Competitions = [],
            Points = [],
            Teams = [],
        })
        { }

        public override PlayerBuilder Clone()
            => new PlayerBuilder()
            .SetId(_model.Id)
            .SetName(_model.Name)
            .SetMatches(_model.Matches)
            .SetWonMatches(_model.WonMatches)
            .SetCompetitions(_model.Competitions)
            .SetPoints(_model.Points)
            .SetTeams(_model.Teams);

        public PlayerBuilder SetPoints(List<Point> points)
        {
            _model.Points = points;
            return this;
        }

        public PlayerBuilder SetTeams(List<Team> teams)
        {
            _model.Teams = teams;
            return this;
        }
    }
}
