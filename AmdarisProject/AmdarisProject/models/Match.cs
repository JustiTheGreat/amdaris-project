using AmdarisProject.models.competition;
using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.exceptions;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.models
{
    public class Match : Model
    {
        public string Location { get; set; }
        public DateTime? StartTime { get; set; }
        public Competitor CompetitorOne { get; set; }
        public Competitor CompetitorTwo { get; set; }
        public MatchStatus Status { get; set; }
        public Competition Competition { get; set; }

        public Match(string location, DateTime? startTime, Competitor competitorOne, Competitor competitorTwo, Competition competition)
        {
            if (competition.GameRules.CompetitorType is CompetitorType.PLAYER
                    && (competitorOne is not Player || competitorTwo is not Player)
                || competition.GameRules.CompetitorType is CompetitorType.TEAM
                    && (competitorOne is not Team || competitorTwo is not Team))
                throw new CompetitorException(MessageFormatter.Format(nameof(Match), nameof(Match),
                    "Competiors not matching the competition type!"));

            if (competitorOne.Equals(competitorTwo))
                throw new CompetitorException(MessageFormatter.Format(nameof(Match), nameof(Match),
                    $"Match with id={Id} has the same competitor ({competitorOne.Name}) on both sides!"));

            if (!competition.ContainsCompetitor(competitorOne)
                || !competition.ContainsCompetitor(competitorTwo))
                throw new CompetitorException(MessageFormatter.Format(nameof(Match), nameof(Match), "Competitor registered to competition!"));

            Location = location;
            StartTime = startTime;
            CompetitorOne = competitorOne;
            CompetitorTwo = competitorTwo;
            Competition = competition;
            Status = MatchStatus.NOT_STARTED;
        }

        public bool ContainsCompetitor(Competitor competitor)
            => competitor.Equals(CompetitorOne)
                || competitor.Equals(CompetitorTwo)
                || competitor is Player player
                    && (((CompetitorOne as Team)?.ContainsPlayer(player) ?? false)
                        || ((CompetitorTwo as Team)?.ContainsPlayer(player) ?? false));

        public void Start()
        {
            if (Competition.Status is not CompetitionStatus.STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Match), nameof(Start), Competition.Status.ToString()));

            if (Status is not MatchStatus.NOT_STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Match), nameof(Start), Status.ToString()));

            DateTime now = DateTime.Now;
            bool lateStart = now > StartTime;
            StartTime = now;

            //extract to method
            if (lateStart)
            {
                int i = 0;
                Competition.Matches
                    .Where(match => match.Status is MatchStatus.NOT_STARTED)
                    .OrderBy(match => match.StartTime)
                    .ToList()
                    .ForEach(match => match.StartTime = now.AddSeconds(
                        (double)(++i * (Competition.GameRules.DurationInSeconds! + Competition.GameRules.BreakInSeconds)))
                    );
            }

            CompetitorOne.InitializePointsForMatch(this);
            CompetitorTwo.InitializePointsForMatch(this);

            Status = MatchStatus.STARTED;
            Console.WriteLine($"Competition {Competition.Name}: Match between {CompetitorOne.Name} and {CompetitorTwo.Name} has started!");
        }

        public void End()
        {
            if (Status is not MatchStatus.STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Match), nameof(End), Status.ToString()));

            if (CompetitorOne.GetPoints(this) != Game.WinAt && CompetitorTwo.GetPoints(this) != Game.WinAt)
                throw new PointsException(MessageFormatter.Format(nameof(Match), nameof(End), "Not enough points to end the match!"));

            Status = MatchStatus.FINISHED;
            Console.WriteLine($"Competition {Competition.Name}: Match between {CompetitorOne.Name} and {CompetitorTwo.Name} has ended with score {CompetitorOne.GetPoints(this)}-{CompetitorTwo.GetPoints(this)}!");

            bool allMatchesWerePlayed = !Competition.GetUnfinishedMatches().Any();
            if (allMatchesWerePlayed)
                Competition.CreateBonusMatches();
        }

        public void Cancel()
        {
            if (Status is not MatchStatus.NOT_STARTED or MatchStatus.STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Match), nameof(Cancel), Status.ToString()));

            Status = MatchStatus.CANCELED;
        }

        public void SpecialWinCompetitorOne()
        {
            if (Status is not MatchStatus.STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Match), nameof(SpecialWinCompetitorOne), Status.ToString()));

            Status = MatchStatus.SPECIAL_WIN_COMPETITOR_ONE;
        }

        public void SpecialWinCompetitorTwo()
        {
            if (Status is not MatchStatus.STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Match), nameof(SpecialWinCompetitorTwo), Status.ToString()));

            Status = MatchStatus.SPECIAL_WIN_COMPETITOR_TWO;
        }

        public int GetPointsCompetitorOne()
        {
            if (Status is MatchStatus.NOT_STARTED or MatchStatus.CANCELED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Match), nameof(GetPointsCompetitorOne), Status.ToString()));

            return CompetitorOne.GetPoints(this);
        }

        public int GetPointsCompetitorTwo()
        {
            if (Status is MatchStatus.NOT_STARTED or MatchStatus.CANCELED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Match), nameof(GetPointsCompetitorTwo), Status.ToString()));

            return CompetitorTwo.GetPoints(this);
        }

        public Competitor? GetWinner()
        {
            if (Status is MatchStatus.NOT_STARTED or MatchStatus.STARTED or MatchStatus.CANCELED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Match), nameof(GetWinner), Status.ToString()));

            if (Status is MatchStatus.SPECIAL_WIN_COMPETITOR_ONE)
                return CompetitorOne;

            if (Status is MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                return CompetitorTwo;

            return CompetitorOne.GetPoints(this) == CompetitorTwo.GetPoints(this) ? null
                : CompetitorOne.GetPoints(this) > CompetitorTwo.GetPoints(this) ? CompetitorOne
                : CompetitorTwo;
        }
    }
}
