using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models.CompetitionModels;

namespace AmdarisProject.Domain.Extensions
{
    public static class CompetitionCantContinue
    {
        public static bool CantContinue(this Competition competition)
            => competition is OneVSAllCompetition oneVSAllCompetition
                    && oneVSAllCompetition.Matches.All(match => match.Status is MatchStatus.CANCELED)
                || competition is TournamentCompetition tournamentCompetition
                    && tournamentCompetition.StageLevel > 0
                    && competition.Matches
                        .Where(match => match.StageLevel == tournamentCompetition.StageLevel)
                        .Any()
                    && competition.Matches
                        .Where(match => match.StageLevel == tournamentCompetition.StageLevel)
                        .All(match => match.Status is MatchStatus.CANCELED);
    }
}
