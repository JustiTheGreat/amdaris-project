using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Abstractions.RepositoryAbstractions
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        Task<IEnumerable<Match>> GetNotStartedByCompetitionOrderedByStartTime(Guid competitionId);

        Task<double> GetCompetitorWinRatingForGameType(Guid competitorId, Guid gameTypeId);
    }
}
