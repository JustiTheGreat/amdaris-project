using AmdarisProject.Application.Abstractions;
using AmdarisProject.Application.ExtensionMethods;
using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AmdarisProject.Infrastructure.Repositories
{
    public class MatchRepository(AmdarisProjectDBContext dbContext)
        : GenericRepository<Match>(dbContext), IMatchRepository
    {
        public async Task<Match?> GetMatchByCompetitionStageLevelAndStageIndex(Guid competitionId, ushort stageLevel, ushort stageIndex)
            => await _dbContext.Set<Match>()
            .Where(match =>
                match.Competition.Id.Equals(competitionId)
                && match.StageLevel == stageLevel
                && match.StageIndex == stageIndex)
            .FirstOrDefaultAsync();

        public async Task<Match?> GetMatchByCompetitionAndTheTwoCompetitors(Guid competitionId, Guid competitorId1, Guid competitorId2)
            => await _dbContext.Set<Match>()
            .Where(match =>
                match.Competition.Id.Equals(competitionId)
                && (match.CompetitorOne.Id.Equals(competitorId1) && match.CompetitorTwo.Id.Equals(competitorId2)
                    || match.CompetitorOne.Id.Equals(competitorId2) && match.CompetitorTwo.Id.Equals(competitorId1)))
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<Match>> GetAllByCompetitionAndStageLevel(Guid competitionId, ushort stageLevel)
            => await _dbContext.Set<Match>()
            .Where(match => match.Competition.Id.Equals(competitionId) && match.StageLevel == stageLevel)
            .OrderBy(match => match.StageIndex)
            .ToListAsync();

        public async Task<bool> AllMatchesOfCompetitonAreFinished(Guid competitionId)
            => await _dbContext.Set<Match>()
            .Where(match => match.Competition.Id.Equals(competitionId))
            .AllAsync(match => match.Status != MatchStatus.NOT_STARTED && match.Status != MatchStatus.STARTED);

        public async Task<IEnumerable<Match>> GetAllByCompetitorAndGameType(Guid competitorId, GameType gameType)
            => (await _dbContext.Set<Match>().Where(match => match.Competition.GameFormat.GameType == gameType)
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
                .Where(match => match.Competition.GameFormat.GameType == gameType && match.Winner != null)
                .Include(match => match.Winner)
                .ToListAsync())
                .Where(match => match.Winner!.IsOrContainsCompetitor(competitorId))
                .Count();
            var winRating = (playedMatchesOfGameType == 0 ? 0 : wonMatchedOfGameType / (double)playedMatchesOfGameType);
            return winRating;
        }
    }
}
