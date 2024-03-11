using AmdarisProject.models.competitor;
using AmdarisProject.utils;

namespace AmdarisProject.models.competition
{
    public class TournamentCompetition : Competition
    {
        public IEnumerable<Stage> Stages { get; set; }

        public TournamentCompetition(string name, string location, DateTime startTime, GameType gameType, CompetitorType competitorType)
            : base(name, location, startTime, gameType, competitorType)
        {
            Stages = [];
        }

        private TournamentCompetition(int id, string name, string location, DateTime startTime, GameType gameType, CompetitorType competitorType,
            CompetitionStatus status, IEnumerable<Competitor> competitors, IEnumerable<Match> matches, IEnumerable<Stage> stages)
            : base(id, name, location, startTime, gameType, competitorType, status, competitors, matches)
        {
            Stages = stages;
        }

        public override object Clone()
        {
            return new TournamentCompetition(
               Id,
               Name,
               Location,
               StartTime,
               GameType,
               CompetitorType,
               Status,
               Competitors,
               Matches,
               Stages
               );
        }
    }
}
