using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Extensions
{
    public static class AllMatchesOfCompetionAreFinished
    {
        public static bool AllMatchesOfCompetitonAreDone(this Competition competition)
            => competition.Matches
            .All(match => match.Status == MatchStatus.FINISHED
                || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_TWO
                || match.Status == MatchStatus.CANCELED);
    }
}
