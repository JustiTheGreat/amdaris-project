using AmdarisProject.models.competitor;
using AmdarisProject.utils.enums;

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

        public void PrintStages(int fromIndex = 0, Competitor? competitor = null)
        {
            if (fromIndex < 0 || fromIndex >= Stages.Count())
                Console.WriteLine("Stage number is not ok");
            for (int i = fromIndex; i < Stages.Count(); i++)
            {
                if (competitor is not null && Stages.ElementAt(i).ContainsCompetitor(competitor))
                    Console.WriteLine(Stages.ElementAt(i));
                else Console.WriteLine(Stages.ElementAt(i));
            }
        }
    }
}
