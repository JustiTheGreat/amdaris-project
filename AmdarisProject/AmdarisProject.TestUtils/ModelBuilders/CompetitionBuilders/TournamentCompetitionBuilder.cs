﻿using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.TestUtils.ModelBuilders.CompetitionBuilders
{
    public class TournamentCompetitionBuilder : CompetitionBuilder<TournamentCompetition, TournamentCompetitionBuilder>
    {
        public TournamentCompetitionBuilder() : base(new TournamentCompetition()
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Location = "Test",
            StartTime = DateTime.UtcNow,
            Status = CompetitionStatus.ORGANIZING,
            BreakInMinutes = null,
            GameFormat = APBuilder.CreateBasicGameFormat().Get(),
            Competitors = [],
            Matches = [],
            StageLevel = 0
        })
        { }

        public override TournamentCompetitionBuilder Clone()
            => new TournamentCompetitionBuilder()
            .SetId(_model.Id)
            .SetName(_model.Name)
            .SetLocation(_model.Location)
            .SetStartTime(_model.StartTime)
            .SetStatus(_model.Status)
            .SetBreakInMinutes(_model.BreakInMinutes)
            .SetGameFormat(_model.GameFormat)
            .SetCompetitors(_model.Competitors)
            .SetMatches(_model.Matches)
            .SetStageLevel(_model.StageLevel);

        public TournamentCompetitionBuilder SetStageLevel(uint stageLevel)
        {
            _model.StageLevel = stageLevel;
            return this;
        }
    }
}
