using AmdarisProject.models.competitor;
using Domain.Enums;

namespace AmdarisProject.models.competition
{
    public class OneVSAllCompetition : Competition
    {
        public OneVSAllCompetition()
        {
        }

        public OneVSAllCompetition(string name, string location, DateTime startTime, CompetitionStatus status,
            uint? winAt, ulong? durationInSeconds, ulong? breakInSeconds, GameType gameType, CompetitorType competitorType, ushort? teamSize,
            List<Competitor> competitors, List<Match> matches)
            : base(name, location, startTime, status, winAt, durationInSeconds, breakInSeconds, gameType, competitorType, teamSize,
                  competitors, matches)
        {
        }
    }
}
