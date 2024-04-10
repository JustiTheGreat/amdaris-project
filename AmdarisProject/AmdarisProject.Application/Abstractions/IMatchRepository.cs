using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Abstractions
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        Task<bool> ContainsCompetitor(ulong matchId, ulong competitorId);

        Task<IEnumerable<Match>> GetUnfinishedByCompetition(ulong competitionId);

        Task<IEnumerable<Match>> GetAllByCompetitorAndGameType(ulong competitorId, GameType gameType);

        Task<IEnumerable<Match>> GetAllByCompetitorAndCompetition(ulong competitorId, ulong competitionId);

        Task<IEnumerable<Match>> GetNotStartedByCompetitionOrderedByStartTime(ulong competitionId);
    }
}
