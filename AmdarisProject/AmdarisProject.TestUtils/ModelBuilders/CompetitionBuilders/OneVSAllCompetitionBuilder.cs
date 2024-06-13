using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.TestUtils.ModelBuilders.CompetitionBuilders
{
    public class OneVSAllCompetitionBuilder : CompetitionBuilder<OneVSAllCompetition, OneVSAllCompetitionBuilder>
    {
        public OneVSAllCompetitionBuilder(DateTime initialStartTime) : base(new OneVSAllCompetition()
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Location = "Test",
            InitialStartTime = initialStartTime,
            ActualizedStartTime = initialStartTime,
            Status = CompetitionStatus.ORGANIZING,
            BreakInMinutes = null,
            GameFormat = APBuilder.CreateBasicGameFormat().Get(),
            Competitors = [],
            Matches = [],
        })
        { }

        public override OneVSAllCompetitionBuilder Clone()
            => new OneVSAllCompetitionBuilder(DateTime.UtcNow)
            .SetId(_model.Id)
            .SetName(_model.Name)
            .SetLocation(_model.Location)
            .SetInitialStartTime(_model.InitialStartTime)
            .SetActualizedStartTime(_model.ActualizedStartTime)
            .SetStatus(_model.Status)
            .SetBreakInMinutes(_model.BreakInMinutes)
            .SetGameFormat(_model.GameFormat)
            .SetCompetitors(_model.Competitors)
            .SetMatches(_model.Matches);
    }
}
