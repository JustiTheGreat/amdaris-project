using AmdarisProject.utils.enums;

namespace AmdarisProject.models.competition
{
    public class OneVSAllCompetition : Competition
    {
        public OneVSAllCompetition(string name, string location, DateTime startTime, GameType gameType, CompetitorType competitorType)
            : base(name, location, startTime, gameType, competitorType)
        {
        }
    }
}
