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
                StartTime = DateTime.Now,
                Status = CompetitionStatus.ORGANIZING,
                WinAt = 2,
                DurationInSeconds = 10,
                BreakInSeconds = 10,
                GameType = GameType.PING_PONG,
                CompetitorType = CompetitorType.PLAYER,
                TeamSize = null,
            });
    }
}
