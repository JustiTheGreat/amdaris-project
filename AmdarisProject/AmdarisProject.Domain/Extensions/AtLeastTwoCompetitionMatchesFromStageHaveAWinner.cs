using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Extensions
{
    public static class AtLeastTwoTournamentCompetitionMatchesFromTheCurrentStageHaveAWinner
    {
        public static bool AtLeastTwoMatchesFromTheCurrentStageHaveAWinner(this TournamentCompetition tournamentCompetition)
            => tournamentCompetition.Matches
            .Count(match => match.StageLevel == tournamentCompetition.StageLevel
                && (match.Status == MatchStatus.FINISHED
                    || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                    || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)) >= 2;
    }
}
