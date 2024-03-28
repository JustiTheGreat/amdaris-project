using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;

namespace AmdarisProject.models.competition
{
    public class TournamentCompetition(string name, string location, DateTime startTime, GameRules gameRules, CompetitionStatus status,
        List<Competitor> competitors, List<Match> matches, List<Stage> stages)
        : Competition(name, location, startTime, gameRules, status, competitors, matches)
    {
        public IEnumerable<Stage> Stages { get; set; } = stages;
    }
}
