using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Extensions
{
    public static class GetMatchWinner
    {
        public static Competitor? GetWinner(this Match match)
            => match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_ONE ? match.CompetitorOne
            : match.Status is MatchStatus.SPECIAL_WIN_COMPETITOR_TWO ? match.CompetitorTwo
            : match.Status is MatchStatus.FINISHED ?
                match.CompetitorOnePoints > match.CompetitorTwoPoints ? match.CompetitorOne
                : match.CompetitorOnePoints < match.CompetitorTwoPoints ? match.CompetitorTwo
                : null
            : throw new APIllegalStatusException(match.Status);
    }
}
