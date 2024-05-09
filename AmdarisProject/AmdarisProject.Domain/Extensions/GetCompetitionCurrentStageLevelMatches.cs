using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Extensions
{
    public static class GetCompetitionCurrentStageLevelMatches
    {
        public static IEnumerable<Match> GetCurrentStageLevelMatches(this TournamentCompetition tournamentCompetition)
            => tournamentCompetition.Matches
            .Where(match => match.StageLevel == tournamentCompetition.StageLevel)
            .OrderBy(match => match.StageIndex)
            .ToList();
    }
}
