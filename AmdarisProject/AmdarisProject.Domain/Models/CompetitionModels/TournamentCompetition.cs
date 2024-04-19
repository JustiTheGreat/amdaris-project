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

        public TournamentCompetition(string name, string location, DateTime startTime, CompetitionStatus status, uint? winAt,
            ulong? durationInSeconds, ulong? breakInSeconds, GameType gameType, CompetitorType competitorType, ushort? teamSize,
            ushort stageLevel, List<Competitor> competitors, List<Match> matches)
            : base(name, location, startTime, status, winAt, durationInSeconds, breakInSeconds, gameType, competitorType, teamSize,
                  competitors, matches)
        {
            StageLevel = stageLevel;
        }
    }
}
