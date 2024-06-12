using AmdarisProject.Application.Abstractions.RepositoryAbstractions;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using AmdarisProject.Domain.Models.CompetitorModels;
using AmdarisProject.Infrastructure.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Persistance.Repositories
{
    public class MatchRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Match>(dbContext), IMatchRepository
    {
        public new async Task<Match?> GetById(Guid id)
            => await _dbContext.Set<Match>()
            .AsSplitQuery()
            .Include(o => o.CompetitorOne).ThenInclude(o => ((Team)o).TeamPlayers).ThenInclude(o => o.Player)
            .Include(o => o.CompetitorTwo).ThenInclude(o => ((Team)o).TeamPlayers).ThenInclude(o => o.Player)
            .Include(o => o.Competition).ThenInclude(o => o.GameFormat)
            .Include(o => o.Competition).ThenInclude(o => o.Matches)
            .Include(o => o.Winner)
            .Include(o => o.Points)
            .FirstOrDefaultAsync(item => item.Id.Equals(id));

        public async Task<IEnumerable<Match>> GetNotStartedByCompetitionOrderedByStartTime(Guid competitionId)
            => await _dbContext.Set<Match>()
                .Where(match => match.Competition.Id.Equals(competitionId) && match.Status == MatchStatus.NOT_STARTED)
                .OrderBy(match => match.StartTime)
                .ToListAsync();

        public async Task<double> GetCompetitorWinRatingForGameType(Guid competitorId, Guid gameTypeId)
        {
            int playedMatchesOfGameType = await _dbContext.Set<Match>()
                .CountAsync(match => match.Competition.GameFormat.GameType.Id.Equals(gameTypeId)
                    && (match.CompetitorOne.Id.Equals(competitorId)
                        || match.CompetitorTwo.Id.Equals(competitorId)
                        || match.CompetitorOne is Team
                            && ((Team)match.CompetitorOne).Players.Any(player => player.Id.Equals(competitorId))
                        || match.CompetitorTwo is Team
                            && ((Team)match.CompetitorTwo).Players.Any(player => player.Id.Equals(competitorId))));

            int wonMatchedOfGameType = await _dbContext.Set<Match>()
                .CountAsync(match => match.Competition.GameFormat.GameType.Id.Equals(gameTypeId)
                    && match.Winner != null
                    && (match.Winner.Id.Equals(competitorId)
                        || match.Winner is Team
                            && ((Team)match.Winner).Players.Any(player => player.Id.Equals(competitorId))));

            double winRating = playedMatchesOfGameType == 0 ? 0 : wonMatchedOfGameType / (double)playedMatchesOfGameType;
            return winRating;
        }

        //public async Task<Dictionary<Guid, Tuple<int, int>>> GetCompetitorWinRatings(Guid competitorId, List<Guid> gameTypeIds)
        //{
        //    return _dbContext.Set<Match>().Aggregate(new Dictionary<Guid, Tuple<int, int>>(),
        //    (result, match) =>
        //    {
        //        if (match.CompetitorOne.Id.Equals(competitorId)
        //            || match.CompetitorTwo.Id.Equals(competitorId)
        //            || match.CompetitorOne is Team
        //                && ((Team)match.CompetitorOne).Players.Any(player => player.Id.Equals(competitorId))
        //            || match.CompetitorTwo is Team
        //                && ((Team)match.CompetitorTwo).Players.Any(player => player.Id.Equals(competitorId)))
        //        {
        //            bool wonIt = match.Winner != null
        //                && (match.Winner.Id.Equals(competitorId)
        //                    || match.Winner is Team
        //                        && ((Team)match.Winner).Players.Any(player => player.Id.Equals(competitorId)));

        //            Guid matchGameTypeId = match.Competition.GameFormat.GameType.Id;
        //            Tuple<int, int>? value = result.GetValueOrDefault(matchGameTypeId);
        //            result.Add(matchGameTypeId, value == null ? Tuple.Create(1, wonIt ? 1 : 0)
        //                : Tuple.Create(value.Item1 + 1, value.Item2 + (wonIt ? 1 : 0)));
        //        }
        //        return result;
        //    });
        //}
    }
}
