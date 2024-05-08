using AmdarisProject.Application.Test.ModelBuilder.ModelBuilder;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
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
                StartTime = null,
                EndTime = null,
                Status = MatchStatus.NOT_STARTED,
                CompetitorOne = Builders.CreateBasicPlayer().Get(),
                CompetitorTwo = Builders.CreateBasicPlayer().Get(),
                Competition = Builders.CreateBasicOneVSAllCompetition().Get(),
                CompetitorOnePoints = null,
                CompetitorTwoPoints = null,
                Winner = null,
                StageLevel = null,
                StageIndex = null,
                Points = []
            });

        public MatchBuilder Clone()
            => new(new Match()
            {
                Id = _model.Id,
                Location = _model.Location,
                StartTime = _model.StartTime,
                EndTime = _model.EndTime,
                Status = _model.Status,
                CompetitorOne = _model.CompetitorOne,
                CompetitorTwo = _model.CompetitorTwo,
                Competition = _model.Competition,
                CompetitorOnePoints = _model.CompetitorOnePoints,
                CompetitorTwoPoints = _model.CompetitorTwoPoints,
                Winner = _model.Winner,
                StageLevel = _model.StageLevel,
                StageIndex = _model.StageIndex,
                Points = _model.Points
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

        public MatchBuilder SetCompetition(Competition competition)
        {
            _model.Competition = competition;
            return this;
        }

        public MatchBuilder SetCompetitorOnePoints(uint? points)
        {
            _model.CompetitorOnePoints = points;
            return this;
        }

        public MatchBuilder SetCompetitorTwoPoints(uint? points)
        {
            _model.CompetitorTwoPoints = points;
            return this;
        }

        public MatchBuilder InitializePoints()
        {
            void CreatePoint(Player player)
                => _model.Points.Add(Builders.CreateBasicPoint().SetPlayer(player).SetMatch(_model).SetValue(0).Get());

            void CreatePointsForTeamPlayers(Team team) => team.Players.ForEach(CreatePoint);

            if (_model.Competition.GameFormat.CompetitorType is CompetitorType.PLAYER)
            {
                CreatePoint((Player)_model.CompetitorOne);
                CreatePoint((Player)_model.CompetitorTwo);
            }
            else
            {
                CreatePointsForTeamPlayers((Team)_model.CompetitorOne);
                CreatePointsForTeamPlayers((Team)_model.CompetitorTwo);
            }

            return this;
        }
    }
}
