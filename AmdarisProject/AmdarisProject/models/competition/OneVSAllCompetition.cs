using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;

namespace AmdarisProject.models.competition
{
    public class OneVSAllCompetition(string name, string location, DateTime startTime, GameRules gameRules, CompetitionStatus status,
        List<Competitor> competitors, List<Match> matches)
        : Competition(name, location, startTime, gameRules, status, competitors, matches)
    {
    }
}
