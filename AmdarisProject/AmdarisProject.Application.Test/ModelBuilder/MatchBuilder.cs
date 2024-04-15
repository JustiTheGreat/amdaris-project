using AmdarisProject.Application.Test.ModelBuilder.ModelBuilder;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Test.ModelBuilder
{
    internal class MatchBuilder : ModelBuilder<Match, MatchBuilder>
    {
        private MatchBuilder(Match match) : base(match) { }

        public static MatchBuilder CreateBasic()
            => new(new Match()
            {
                Id = Guid.NewGuid(),
                Location = "Test",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Status = Domain.Enums.MatchStatus.FINISHED,
                CompetitorOne = Builders.CreateBasicPlayer().Get(),
                CompetitorTwo = Builders.CreateBasicPlayer().Get(),
                Competition = Builders.CreateBasicOneVSAllCompetition().Get(),
                Stage = null,
                Points = []
            });

        public MatchBuilder SetStatus(MatchStatus status)
        {
            _model.Status = status;
            return this;
        }

        public MatchBuilder SetCompetitorOne(Competitor competitor)
        {
            _model.CompetitorOne = competitor;
            return this;
        }
    }
}
