using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Application.Test.ModelBuilder.CompetitionBuilders
{
    internal class OneVSAllCompetitionBuilder : CompetitionBuilder<OneVSAllCompetition, OneVSAllCompetitionBuilder>
    {
        private OneVSAllCompetitionBuilder(OneVSAllCompetition oneVSAllCompetition) : base(oneVSAllCompetition) { }

        public static OneVSAllCompetitionBuilder CreateBasic()
            => new(new OneVSAllCompetition()
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Location = "Test",
                StartTime = DateTime.UtcNow,
                Status = CompetitionStatus.ORGANIZING,
                BreakInMinutes = 3,
                GameFormat = Builders.CreateBasicGameFormat().Get(),
                Competitors = [],
                Matches = [],
            });
    }
}
