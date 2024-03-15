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
        public DateTime StartTime { get; set; }
        public Competitor CompetitorOne { get; set; }
        public Competitor CompetitorTwo { get; set; }
        public Game Game { get; set; }
        public MatchStatus Status { get; set; }
        public Competition Competition { get; set; }

        public Match(string location, DateTime startTime, Game game, Competitor competitorOne, Competitor competitorTwo, Competition competition)
        {
            if (game.CompetitorType is CompetitorType.PLAYER && (competitorOne is not Player || competitorTwo is not Player)
                || game.CompetitorType is CompetitorType.TWO_PLAYER_TEAM && (competitorOne is not TwoPlayerTeam || competitorTwo is not TwoPlayerTeam))
                throw new CompetitorException(MessageFormatter.Format(nameof(Match), nameof(Match), "Competiors not matching the competition type!"));

            if (competitorOne.Equals(competitorTwo))
                throw new SameCompetitorException(MessageFormatter.Format(nameof(Match), nameof(Match), competitorOne.Name));

            if (!competition.ContainsCompetitor(competitorOne) || !competition.ContainsCompetitor(competitorTwo))
                throw new CompetitorException(MessageFormatter.Format(nameof(Match), nameof(Match), "Competior not in competition!"));

            Location = location;
            StartTime = startTime;
            Game = game;
            CompetitorOne = competitorOne;
            CompetitorTwo = competitorTwo;
            Competition = competition;
            Status = MatchStatus.NOT_STARTED;
        }

        public bool ContainsCompetitor(Competitor competitor)
        {
            if (Game.CompetitorType is CompetitorType.PLAYER && competitor is not Player)
                throw new CompetitorException(MessageFormatter.Format(nameof(Match), nameof(ContainsCompetitor), "Competitor not matching the competition type!"));

            if (competitor is Player)
            {
                return CompetitorOne is not null
                            && (CompetitorOne is Player && competitor.Equals(CompetitorOne)
                                || CompetitorOne is TwoPlayerTeam && (CompetitorOne as TwoPlayerTeam).ContainsPlayer(competitor as Player))
                        || CompetitorTwo is not null
                            && (CompetitorTwo is Player && competitor.Equals(CompetitorTwo)
                                || CompetitorTwo is TwoPlayerTeam && (CompetitorTwo as TwoPlayerTeam).ContainsPlayer(competitor as Player));
            }
            else
            {
                return CompetitorOne is not null && competitor.Equals(CompetitorOne)
                    || CompetitorTwo is not null && competitor.Equals(CompetitorTwo);
            }
        }

        public void Start()
        {
            if (Competition.Status is not CompetitionStatus.STARTED || Status is not MatchStatus.NOT_STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Match), nameof(Start), Status.ToString()));

            CompetitorOne.InitializePointsForMatch(this);
            CompetitorTwo.InitializePointsForMatch(this);

            Status = MatchStatus.STARTED;
            Console.WriteLine($"Competition {Competition.Name}: Match between {CompetitorOne.Name} and {CompetitorTwo.Name} has statrted!");
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
            if (Status is MatchStatus.NOT_STARTED or MatchStatus.STARTED or MatchStatus.CANCELED or MatchStatus.DRAW)
                return null;

            if (Status is MatchStatus.SPECIAL_WIN_COMPETITOR_ONE)
                return CompetitorOne;

            if (Status is MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                return CompetitorTwo;

            if (CompetitorOne.GetPoints(this) == CompetitorTwo.GetPoints(this))
                throw new DrawMatchResultException(MessageFormatter.Format(nameof(Match), nameof(GetPointsCompetitorTwo), Id.ToString()));

            return CompetitorOne.GetPoints(this) > CompetitorTwo.GetPoints(this) ? CompetitorOne : CompetitorTwo;
        }
    }
}
