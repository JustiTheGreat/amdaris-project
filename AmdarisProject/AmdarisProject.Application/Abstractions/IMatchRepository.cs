using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Abstractions
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        Task<bool> ContainsCompetitor(Guid matchId, Guid competitorId);

        Task<IEnumerable<Match>> GetUnfinishedByCompetition(Guid competitionId);

        Task<IEnumerable<Match>> GetAllByCompetitorAndGameType(Guid competitorId, GameType gameType);

        Task<IEnumerable<Match>> GetAllByCompetitorAndCompetition(Guid competitorId, Guid competitionId);

        Task<IEnumerable<Match>> GetNotStartedByCompetitionOrderedByStartTime(Guid competitionId);
    }
}
