using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Application.Test.ModelBuilders.CompetitionBuilders
{
    public class OneVSAllCompetitionBuilder : CompetitionBuilder<OneVSAllCompetition, OneVSAllCompetitionBuilder>
    {
        public OneVSAllCompetitionBuilder() : base(new OneVSAllCompetition()
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Location = "Test",
            StartTime = DateTime.UtcNow,
            Status = CompetitionStatus.ORGANIZING,
            BreakInMinutes = 3,
            GameFormat = APBuilder.CreateBasicGameFormat().Get(),
            Competitors = [],
            Matches = [],
        })
        { }

        public override OneVSAllCompetitionBuilder Clone()
            => new OneVSAllCompetitionBuilder()
            .SetId(_model.Id)
            .SetName(_model.Name)
            .SetLocation(_model.Location)
            .SetStartTime(_model.StartTime)
            .SetStatus(_model.Status)
            .SetBreakInMinutes(_model.BreakInMinutes)
            .SetGameFormat(_model.GameFormat)
            .SetCompetitors(_model.Competitors)
            .SetMatches(_model.Matches);
    }
}
