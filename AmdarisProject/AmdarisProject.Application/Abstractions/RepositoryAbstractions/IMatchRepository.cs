using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Abstractions.RepositoryAbstractions
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        Task<Match?> GetMatchByCompetitionAndTheTwoCompetitors(Guid competitionId, Guid competitorId1, Guid competitorId2);

        Task<IEnumerable<Match>> GetNotStartedByCompetitionOrderedByStartTime(Guid competitionId);

        Task<double> GetCompetitorWinRatingForGameType(Guid competitorId, GameType gameType);

        Task<bool> CompetitorIsInAStartedMatch(Guid competitorId);
    }
}
