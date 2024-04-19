using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Models;

namespace AmdarisProject.Application.Abstractions
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        Task<Match?> GetMatchByCompetitionStageLevelAndStageIndex(Guid competitionId, ushort stageLevel, ushort stageIndex);

        Task<Match?> GetMatchByCompetitionAndTheTwoCompetitors(Guid competitionId, Guid competitorId1, Guid competitorId2);

        Task<IEnumerable<Match>> GetAllByCompetitionAndStageLevel(Guid competitionId, ushort stageLevel);

        Task<bool> AllMatchesOfCompetitonAreFinished(Guid competitionId);

        Task<IEnumerable<Match>> GetAllByCompetitorAndGameType(Guid competitorId, GameType gameType);

        Task<IEnumerable<Match>> GetAllByCompetitorAndCompetition(Guid competitorId, Guid competitionId);

        Task<IEnumerable<Match>> GetNotStartedByCompetitionOrderedByStartTime(Guid competitionId);

        Task<double> GetCompetitorWinRatingForGameType(Guid competitorId, GameType gameType);
    }
}
