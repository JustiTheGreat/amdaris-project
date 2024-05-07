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
                Id = Guid.NewGuid(),
                Name = "Test",
                Location = "Test",
                StartTime = DateTime.UtcNow,
                Status = CompetitionStatus.ORGANIZING,
                BreakInMinutes = 3,
                GameFormat = Builders.CreateBasicGameFormat().Get(),
                Competitors = [],
                Matches = [],
                StageLevel = 0
            });
    }
}
