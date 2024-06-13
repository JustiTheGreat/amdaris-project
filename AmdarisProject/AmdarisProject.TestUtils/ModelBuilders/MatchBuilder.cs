using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.TestUtils.ModelBuilders
{
    public class MatchBuilder : ModelBuilder<Match, MatchBuilder>
    {
        public MatchBuilder() : base(new Match()
        {
            Id = Guid.NewGuid(),
            Location = "Test",
            InitialStartTime = null,
            ActualizedStartTime = null,
            InitialEndTime = null,
            ActualizedEndTime = null,
            Status = MatchStatus.NOT_STARTED,
            CompetitorOne = APBuilder.CreateBasicPlayer().Get(),
            CompetitorTwo = APBuilder.CreateBasicPlayer().Get(),
            Competition = APBuilder.CreateBasicOneVSAllCompetition().Get(),
            CompetitorOnePoints = null,
            CompetitorTwoPoints = null,
            Winner = null,
            StageLevel = null,
            StageIndex = null,
            Points = []
        })
        { }

        public override MatchBuilder Clone()
            => new MatchBuilder()
            .SetId(_model.Id)
            .SetLocation(_model.Location)
            .SetInitialStartTime(_model.InitialStartTime)
            .SetInitialStartTime(_model.ActualizedStartTime)
            .SetInitialStartTime(_model.InitialEndTime)
            .SetInitialEndTime(_model.ActualizedEndTime)
            .SetStatus(_model.Status)
            .SetCompetitorOne(_model.CompetitorOne)
            .SetCompetitorTwo(_model.CompetitorTwo)
            .SetCompetition(_model.Competition)
            .SetCompetitorOnePoints(_model.CompetitorOnePoints)
            .SetCompetitorTwoPoints(_model.CompetitorTwoPoints)
            .SetWinner(_model.Winner)
            .SetStageLevel(_model.StageLevel)
            .SetStageIndex(_model.StageIndex)
            .SetPoints(_model.Points);

        public MatchBuilder SetLocation(string location)
        {
            _model.Location = location;
            return this;
        }

        public MatchBuilder SetInitialStartTime(DateTime? initialStartTime)
        {
            _model.InitialStartTime = initialStartTime;
            return this;
        }

        public MatchBuilder SetActualizedStartTime(DateTime? actualizedStartTime)
        {
            _model.ActualizedStartTime = actualizedStartTime;
            return this;
        }

        public MatchBuilder SetInitialEndTime(DateTime? initialEndTime)
        {
            _model.InitialEndTime = initialEndTime;
            return this;
        }

        public MatchBuilder SetActualizedlEndTime(DateTime? actualizedEndTime)
        {
            _model.ActualizedEndTime = actualizedEndTime;
            return this;
        }

        public MatchBuilder SetStatus(MatchStatus status)
        {
            _model.Status = status;
            return this;
        }

        public MatchBuilder SetCompetitorOne(Competitor competitor)
        {
            _model.CompetitorOne = competitor;
            competitor.Matches.Add(_model);
            return this;
        }

        public MatchBuilder SetCompetitorTwo(Competitor competitor)
        {
            _model.CompetitorTwo = competitor;
            competitor.Matches.Add(_model);
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

        public MatchBuilder SetWinner(Competitor? competitor)
        {
            _model.Winner = competitor;
            return this;
        }

        public MatchBuilder SetStageLevel(uint? stageLevel)
        {
            _model.StageLevel = stageLevel;
            return this;
        }

        public MatchBuilder SetStageIndex(uint? stageIndex)
        {
            _model.StageIndex = stageIndex;
            return this;
        }

        public MatchBuilder SetPoints(List<Point> points)
        {
            _model.Points = points;
            return this;
        }

        public MatchBuilder InitializePoints()
        {
            void CreatePoint(Player player)
                => _model.Points.Add(APBuilder.CreateBasicPoint().SetPlayer(player).SetMatch(_model).SetValue(0).Get());

            void CreatePoints(Team team)
                => team.TeamPlayers.ForEach(teamPlayer =>
                {
                    if (teamPlayer.IsActive) CreatePoint(teamPlayer.Player);
                });

            if (_model.Competition.GameFormat.CompetitorType is CompetitorType.PLAYER)
            {
                CreatePoint((Player)_model.CompetitorOne);
                CreatePoint((Player)_model.CompetitorTwo);
            }
            else
            {
                CreatePoints((Team)_model.CompetitorOne);
                CreatePoints((Team)_model.CompetitorTwo);
            }

            SetCompetitorOnePoints(0);
            SetCompetitorTwoPoints(0);

            return this;
        }
    }
}
