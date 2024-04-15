using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.Utils;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Repositories
{
    public class MatchRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Match>(dbContext), IMatchRepository
    {
        public async Task<bool> ContainsCompetitor(Guid matchId, Guid competitorId)
        {
            Match match = await GetById(matchId)
                ?? throw new APNotFoundException(Tuple.Create(nameof(matchId), matchId));

            bool containsCompetitor = HandlerUtils.MatchContainsCompetitor(match, competitorId);
            return containsCompetitor;
        }

        public async Task<IEnumerable<Match>> GetUnfinishedByCompetition(Guid competitionId)
            => await _dbContext.Set<Match>()
            .Where(match => match.Competition.Id.Equals(competitionId)
                && (match.Status == MatchStatus.NOT_STARTED || match.Status == MatchStatus.STARTED))
            .ToListAsync();

        public async Task<IEnumerable<Match>> GetAllByCompetitorAndGameType(Guid competitorId, GameType gameType)
            => await _dbContext.Set<Match>().Where(match => match.Competition.GameType == gameType
            && HandlerUtils.MatchContainsCompetitor(match, competitorId)).ToListAsync();

        public async Task<IEnumerable<Match>> GetAllByCompetitorAndCompetition(Guid competitorId, Guid competitionId)
            => await _dbContext.Set<Match>().Where(match => match.Competition.Id.Equals(competitionId)
            && HandlerUtils.MatchContainsCompetitor(match, competitorId)).ToListAsync();

        public async Task<IEnumerable<Match>> GetNotStartedByCompetitionOrderedByStartTime(Guid competitionId)
            => await _dbContext.Set<Match>()
                .Where(match => match.Competition.Id.Equals(competitionId) && match.Status == MatchStatus.NOT_STARTED)
                .OrderBy(match => match.StartTime)
                .ToListAsync();
    }
}
