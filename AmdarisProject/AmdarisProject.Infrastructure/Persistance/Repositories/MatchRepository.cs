using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public class MatchRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Match>(dbContext), IMatchRepository
    {
        public new async Task<Match?> GetById(Guid id)
            => await _dbContext.Set<Match>()
            .AsSplitQuery()
            .Include(o => o.CompetitorOne)
            .Include(o => o.CompetitorTwo)
            .Include(o => o.Competition)
            .Include(o => o.Winner)
            .Include(o => o.Points)
            .FirstOrDefaultAsync(item => item.Id.Equals(id));

        public async Task<Match?> GetMatchByCompetitionAndTheTwoCompetitors(Guid competitionId, Guid competitorId1, Guid competitorId2)
            => await _dbContext.Set<Match>()
            .Where(match => match.Competition.Id.Equals(competitionId)
                && (match.CompetitorOne.Id.Equals(competitorId1) && match.CompetitorTwo.Id.Equals(competitorId2)
                    || match.CompetitorOne.Id.Equals(competitorId2) && match.CompetitorTwo.Id.Equals(competitorId1)))
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<Match>> GetNotStartedByCompetitionOrderedByStartTime(Guid competitionId)
            => await _dbContext.Set<Match>()
                .Where(match => match.Competition.Id.Equals(competitionId) && match.Status == MatchStatus.NOT_STARTED)
                .OrderBy(match => match.StartTime)
                .ToListAsync();

        public async Task<double> GetCompetitorWinRatingForGameType(Guid competitorId, GameType gameType)
        {
            int playedMatchesOfGameType = await _dbContext.Set<Match>()
                .CountAsync(match => match.Competition.GameFormat.GameType == gameType
                    && (match.CompetitorOne.Id.Equals(competitorId)
                        || match.CompetitorTwo.Id.Equals(competitorId)
                        || match.CompetitorOne is Team
                            && ((Team)match.CompetitorOne).Players.Any(player => player.Id.Equals(competitorId))
                        || match.CompetitorTwo is Team
                            && ((Team)match.CompetitorTwo).Players.Any(player => player.Id.Equals(competitorId))));

            int wonMatchedOfGameType = await _dbContext.Set<Match>()
                .CountAsync(match => match.Competition.GameFormat.GameType == gameType
                    && match.Winner != null
                    && (match.Winner.Id.Equals(competitorId)
                        || match.Winner is Team
                            && ((Team)match.Winner).Players.Any(player => player.Id.Equals(competitorId))));

            double winRating = playedMatchesOfGameType == 0 ? 0 : wonMatchedOfGameType / (double)playedMatchesOfGameType;
            return winRating;
        }

        public async Task<bool> CompetitorIsInAStartedMatch(Guid competitorId)
            => await _dbContext.Set<Match>().AnyAsync(match => match.Status == MatchStatus.STARTED
                && (match.CompetitorOne.Id.Equals(competitorId) || match.CompetitorTwo.Id.Equals(competitorId)));
    }
}
