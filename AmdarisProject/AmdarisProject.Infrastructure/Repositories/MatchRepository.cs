using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.ExtensionMethods;
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

            bool containsCompetitor = match.ContainsCompetitor(competitorId);
            return containsCompetitor;
        }

        public async Task<IEnumerable<Match>> GetUnfinishedByCompetition(Guid competitionId)
            => await _dbContext.Set<Match>()
            .Where(match => match.Competition.Id.Equals(competitionId)
                && (match.Status == MatchStatus.NOT_STARTED || match.Status == MatchStatus.STARTED))
            .ToListAsync();

        public async Task<IEnumerable<Match>> GetAllByCompetitorAndGameType(Guid competitorId, GameType gameType)
            => (await _dbContext.Set<Match>().Where(match => match.Competition.GameType == gameType)
            .Include(match => match.CompetitorOne).Include(match => match.CompetitorTwo).ToListAsync())
            .Where(match => match.ContainsCompetitor(competitorId)).ToList();

        public async Task<IEnumerable<Match>> GetAllByCompetitorAndCompetition(Guid competitorId, Guid competitionId)
            => await _dbContext.Set<Match>().Where(match => match.Competition.Id.Equals(competitionId)
            && match.ContainsCompetitor(competitorId)).ToListAsync();

        public async Task<IEnumerable<Match>> GetNotStartedByCompetitionOrderedByStartTime(Guid competitionId)
            => await _dbContext.Set<Match>()
                .Where(match => match.Competition.Id.Equals(competitionId) && match.Status == MatchStatus.NOT_STARTED)
                .OrderBy(match => match.StartTime)
                .ToListAsync();

        public async Task<double> GetCompetitorWinRatingForGameType(Guid competitorId, GameType gameType)
        {
            int playedMatchesOfGameType = (await GetAllByCompetitorAndGameType(competitorId, gameType)).Count();
            int wonMatchedOfGameType = (await _dbContext.Set<Match>()
                .Where(match => match.Competition.GameType == gameType && match.Winner != null)
                .Include(match => match.Winner)
                .ToListAsync())
                .Where(match => match.Winner!.IsOrContainsCompetitor(competitorId))
                .Count();
            var winRating = (playedMatchesOfGameType == 0 ? 0 : wonMatchedOfGameType / (double)playedMatchesOfGameType);
            return winRating;
        }
    }
}
