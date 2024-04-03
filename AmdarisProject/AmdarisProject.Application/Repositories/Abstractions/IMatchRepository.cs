using AmdarisProject.models;
using Domain.Enums;

namespace AmdarisProject.repositories.abstractions
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        bool ContainsCompetitor(ulong matchId, ulong competitorId);

        IEnumerable<Match> GetUnfinishedByCompetition(ulong competitionId);

        IEnumerable<Match> GetAllByCompetitorAndGameType(ulong competitorId, GameType gameType);

        IEnumerable<Match> GetAllByCompetitorAndCompetition(ulong competitorId, ulong competitionId);
    }
}
