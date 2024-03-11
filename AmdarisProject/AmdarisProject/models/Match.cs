using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.models
{
    public class Match
    {
        private static int instances = 0;
        public int Id { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public Competitor CompetitorOne { get; set; }
        public Competitor CompetitorTwo { get; set; }
        public GameType GameType { get; set; }
        public MatchStatus Status { get; set; }

        public Match(string location, DateTime startTime, GameType gameType, Competitor competitorOne, Competitor competitorTwo)
        {
            if (competitorOne.Equals(competitorTwo))
                throw new SameCompetitorException($"{competitorOne.Name}");
            Id = ++instances;
            Location = location;
            StartTime = startTime;
            CompetitorOne = competitorOne;
            CompetitorTwo = competitorTwo;
            GameType = gameType;
            Status = MatchStatus.NOT_STARTED;
        }

        public void Start()
        {
            if (Status != MatchStatus.NOT_STARTED)
                throw new IllegalStatusException($"{Start}: {Status}");
            Status = MatchStatus.STARTED;
        }

        public void End()
        {
            if (Status != MatchStatus.STARTED)
                throw new IllegalStatusException($"{End}: {Status}");
            Status = MatchStatus.FINISHED;
        }

        public void Cancel()
        {
            if (Status != MatchStatus.NOT_STARTED || Status != MatchStatus.STARTED)
                throw new IllegalStatusException($"{Cancel}: {Status}");
            Status = MatchStatus.CANCELED;
        }

        public void SpecialWinCompetitorOne()
        {
            if (Status != MatchStatus.STARTED)
                throw new IllegalStatusException($"{SpecialWinCompetitorOne}: {Status}");
            Status = MatchStatus.SPECIAL_WIN_COMPETITOR_ONE;
        }

        public void SpecialWinCompetitorTwo()
        {
            if (Status != MatchStatus.STARTED)
                throw new IllegalStatusException($"{SpecialWinCompetitorTwo}: {Status}");
            Status = MatchStatus.SPECIAL_WIN_COMPETITOR_TWO;
        }

        public int GetPointsCompetitorOne()
        {
            if (Status.Equals(MatchStatus.NOT_STARTED) || Status.Equals(MatchStatus.CANCELED)
                || Status.Equals(MatchStatus.SPECIAL_WIN_COMPETITOR_ONE) || Status.Equals(MatchStatus.SPECIAL_WIN_COMPETITOR_TWO))
                throw new IllegalStatusException($"{GetPointsCompetitorOne}: {Status}");
            return CompetitorOne.GetPoints(this);
        }

        public int GetPointsCompetitorTwo()
        {
            if (Status.Equals(MatchStatus.NOT_STARTED) || Status.Equals(MatchStatus.CANCELED)
                || Status.Equals(MatchStatus.SPECIAL_WIN_COMPETITOR_ONE) || Status.Equals(MatchStatus.SPECIAL_WIN_COMPETITOR_TWO))
                throw new IllegalStatusException($"{GetPointsCompetitorTwo}: {Status}");
            return CompetitorTwo.GetPoints(this);
        }

        public Competitor GetWinner()
        {
            if (CompetitorOne.GetPoints(this) == CompetitorTwo.GetPoints(this))
                throw new DrawGameResultException($"{Id}");
            return CompetitorOne.GetPoints(this) > CompetitorTwo.GetPoints(this) ? CompetitorOne : CompetitorTwo;
        }
    }
}
