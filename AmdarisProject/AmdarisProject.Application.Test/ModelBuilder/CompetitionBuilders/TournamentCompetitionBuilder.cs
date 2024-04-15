using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Application.Test.ModelBuilder.CompetitionBuilders
{
    internal class TournamentCompetitionBuilder : CompetitionBuilder<TournamentCompetition, TournamentCompetitionBuilder>
    {
        private TournamentCompetitionBuilder(TournamentCompetition tournamentCompetition) : base(tournamentCompetition) { }

        public static TournamentCompetitionBuilder CreateBasic()
            => new(new TournamentCompetition()
            {
                Id = ++_instances,
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
