using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models.CompetitionModels
{
    public class OneVSAllCompetition : Competition
    {
        public OneVSAllCompetition() : base()
        {
        }

        public OneVSAllCompetition(string name, string location, DateTime startTime, CompetitionStatus status, ulong? breakInSeconds,
            GameFormat gameFormat, List<Competitor> competitors, List<Match> matches)
            : base(name, location, startTime, status, breakInSeconds, gameFormat, competitors, matches)
        {
        }
    }
}
