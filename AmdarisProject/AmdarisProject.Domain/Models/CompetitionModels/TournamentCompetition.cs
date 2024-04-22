using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models.CompetitionModels
{
    public class TournamentCompetition : Competition
    {
        public ushort StageLevel { get; set; }

        public TournamentCompetition() : base()
        {
        }

        public TournamentCompetition(string name, string location, DateTime startTime, CompetitionStatus status, ulong? breakInSeconds,
            GameFormat gameFormat, List<Competitor> competitors, List<Match> matches, ushort stageLevel)
            : base(name, location, startTime, status, breakInSeconds, gameFormat, competitors, matches)
        {
            StageLevel = stageLevel;
        }
    }
}
