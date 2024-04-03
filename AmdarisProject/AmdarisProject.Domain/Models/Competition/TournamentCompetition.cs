using AmdarisProject.models.competitor;
using Domain.Enums;

namespace AmdarisProject.models.competition
{
    public class TournamentCompetition : Competition
    {
        public IEnumerable<Stage> Stages { get; set; }

        public TournamentCompetition()
        {
        }

        public TournamentCompetition(string name, string location, DateTime startTime, CompetitionStatus status,
            uint? winAt, ulong? durationInSeconds, ulong? breakInSeconds, GameType gameType, CompetitorType competitorType, ushort? teamSize,
            List<Competitor> competitors, List<Match> matches, List<Stage> stages)
            : base(name, location, startTime, status, winAt, durationInSeconds, breakInSeconds, gameType, competitorType, teamSize,
                  competitors, matches)
        {
            Stages = stages;
        }
    }
}
