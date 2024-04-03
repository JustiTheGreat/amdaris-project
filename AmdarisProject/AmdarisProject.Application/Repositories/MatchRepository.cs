using AmdarisProject.models;
using AmdarisProject.repositories.abstractions;
using AmdarisProject.utils;
using Domain.Enums;
using Domain.Exceptions;

namespace AmdarisProject.repositories
{
    public class MatchRepository : GenericRepository<Match>, IMatchRepository
    {
        public override Match Update(Match match)
        {
            if (match is null)
                throw new APArgumentException(nameof(MatchRepository), nameof(Update), nameof(match));

            Match stored = GetById(match.Id);

            if (stored.StartTime is not null && match.StartTime is null)
                throw new APArgumentException(nameof(MatchRepository), nameof(Update), nameof(match.StartTime));

            if (stored.EndTime is not null && match.EndTime is null)
                throw new APArgumentException(nameof(MatchRepository), nameof(Update), nameof(match.EndTime));

            stored.Location = match.Location;
            stored.StartTime = match.StartTime;
            stored.EndTime = match.EndTime;
            return stored;
        }

        public bool ContainsCompetitor(ulong matchId, ulong competitorId)
        {
            Match match = GetById(matchId);
            bool containsCompetitor = Utils.MatchContainsCompetitor(match, competitorId);
            return containsCompetitor;
        }

        public IEnumerable<Match> GetUnfinishedByCompetition(ulong competitionId)
            => _dataSet.Where(match => match.Competition.Id == competitionId
            && match.Status is MatchStatus.NOT_STARTED or MatchStatus.STARTED).ToList();

        public IEnumerable<Match> GetAllByCompetitorAndGameType(ulong competitorId, GameType gameType)
            => _dataSet.Where(match => match.Competition.GameType == gameType
            && Utils.MatchContainsCompetitor(match, competitorId)).ToList();

        public IEnumerable<Match> GetAllByCompetitorAndCompetition(ulong competitorId, ulong competitionId)
            => _dataSet.Where(match => match.Competition.Id == competitionId
            && Utils.MatchContainsCompetitor(match, competitorId)).ToList();
    }
}
