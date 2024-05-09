﻿using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Application.Test.ModelBuilders
{
    public class MatchBuilder : ModelBuilder<Match, MatchBuilder>
    {
        public MatchBuilder() : base(new Match()
        {
            Id = Guid.NewGuid(),
            Location = "Test",
            StartTime = null,
            EndTime = null,
            Status = MatchStatus.NOT_STARTED,
            CompetitorOne = Builder.CreateBasicPlayer().Get(),
            CompetitorTwo = Builder.CreateBasicPlayer().Get(),
            Competition = Builder.CreateBasicOneVSAllCompetition().Get(),
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
            .SetStartTime(_model.StartTime)
            .SetEndTime(_model.EndTime)
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

        public MatchBuilder SetStartTime(DateTime? startTime)
        {
            _model.StartTime = startTime;
            return this;
        }

        public MatchBuilder SetEndTime(DateTime? endTime)
        {
            _model.EndTime = endTime;
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
            return this;
        }

        public MatchBuilder SetCompetitorTwo(Competitor competitor)
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
                => _model.Points.Add(Builder.CreateBasicPoint().SetPlayer(player).SetMatch(_model).SetValue(0).Get());

            void CreatePoints(Team team) => team.Players.ForEach(CreatePoint);

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

            return this;
        }
    }
}
