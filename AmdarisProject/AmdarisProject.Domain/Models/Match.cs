﻿using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitionModels;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models
{
    public class Match : Model
    {
        public required string Location { get; set; }
        public required DateTimeOffset? InitialStartTime { get; set; }
        public required DateTimeOffset? ActualizedStartTime { get; set; }
        public required DateTimeOffset? InitialEndTime { get; set; }
        public required DateTimeOffset? ActualizedEndTime { get; set; }
        public required MatchStatus Status { get; set; }
        public virtual required Competitor CompetitorOne { get; set; }
        public virtual required Competitor CompetitorTwo { get; set; }
        public virtual required Competition Competition { get; set; }
        public required uint? CompetitorOnePoints { get; set; }
        public required uint? CompetitorTwoPoints { get; set; }
        public virtual required Competitor? Winner { get; set; }
        public required uint? StageLevel { get; set; }
        public required uint? StageIndex { get; set; }
        public virtual required List<Point> Points { get; set; } = [];

        public Competitor? GetWinner()
            => Status is MatchStatus.SPECIAL_WIN_COMPETITOR_ONE ? CompetitorOne
            : Status is MatchStatus.SPECIAL_WIN_COMPETITOR_TWO ? CompetitorTwo
            : Status is MatchStatus.FINISHED ?
                CompetitorOnePoints > CompetitorTwoPoints ? CompetitorOne
                : CompetitorOnePoints < CompetitorTwoPoints ? CompetitorTwo
                : null
            : throw new APIllegalStatusException(Status);

        public bool ACompetitorHasTheWinningScore()
            => Competition.GameFormat.WinAt != null
            && (CompetitorOnePoints >= Competition.GameFormat.WinAt
            || CompetitorTwoPoints >= Competition.GameFormat.WinAt);

        public void AddPointsForPlayerSide(Guid playerId, uint points)
        {
            if (Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(Status);

            if (CompetitorOne.IsOrContainsCompetitor(playerId))
                CompetitorOnePoints += points;
            else if (CompetitorTwo.IsOrContainsCompetitor(playerId))
                CompetitorTwoPoints += points;
        }

        public bool Start()
        {
            if (Competition.Status is not CompetitionStatus.STARTED)
                throw new APIllegalStatusException(Competition.Status);

            if (Competition.AMatchIsBeignPlayed())
                throw new APException("Cannot start a match while another one is being played!");

            if (Status is not MatchStatus.NOT_STARTED)
                throw new APIllegalStatusException(Status);

            if (Competition.GameFormat.CompetitorType is CompetitorType.TEAM)
            {
                int requiredNumberOfActivePlayers = (int)Competition.GameFormat.TeamSize!;
                bool teamOneHasTheRequiredNumberOfCompetitors =
                    ((Team)CompetitorOne).HasTheRequiredNumberOfActivePlayers(requiredNumberOfActivePlayers);
                bool teamTwoHasTheRequiredNumberOfCompetitors =
                    ((Team)CompetitorTwo).HasTheRequiredNumberOfActivePlayers(requiredNumberOfActivePlayers);

                Status =
                    teamOneHasTheRequiredNumberOfCompetitors && teamTwoHasTheRequiredNumberOfCompetitors ? MatchStatus.STARTED
                    : !teamOneHasTheRequiredNumberOfCompetitors && teamTwoHasTheRequiredNumberOfCompetitors ? MatchStatus.SPECIAL_WIN_COMPETITOR_TWO
                    : teamOneHasTheRequiredNumberOfCompetitors && !teamTwoHasTheRequiredNumberOfCompetitors ? MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                    : MatchStatus.CANCELED;

                Winner =
                    Status == MatchStatus.SPECIAL_WIN_COMPETITOR_ONE ? CompetitorOne
                    : Status == MatchStatus.SPECIAL_WIN_COMPETITOR_TWO ? CompetitorTwo
                    : Winner;
            }
            else Status = MatchStatus.STARTED;

            bool lateStart = false;

            if (Status is MatchStatus.STARTED)
            {
                DateTimeOffset now = DateTimeOffset.UtcNow;
                lateStart = ActualizedStartTime is not null && ActualizedStartTime < now;
                ActualizedStartTime = now;
                ActualizedEndTime = ((DateTimeOffset)ActualizedStartTime).AddMinutes(Competition.GameFormat.DurationInMinutes ?? 0);
                CompetitorOnePoints = 0;
                CompetitorTwoPoints = 0;
            }

            return lateStart;
        }

        public void EndMatch(MatchStatus endStatus)
        {
            if (endStatus is not MatchStatus.FINISHED
                    && endStatus is not MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                    && endStatus is not MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new APIllegalStatusException(endStatus);

            if (Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(Status);

            if (Competition.GameFormat.WinAt is not null && endStatus is MatchStatus.FINISHED && !ACompetitorHasTheWinningScore())
                throw new APException($"Match {CompetitorOne.Name}-{CompetitorTwo.Name} doesn't have a competitor with the winning number of points!");

            ActualizedEndTime = DateTimeOffset.UtcNow;
            Status = endStatus;
            Winner = GetWinner();
        }

        public void Cancel()
        {
            if (Status is not MatchStatus.NOT_STARTED && Status is not MatchStatus.STARTED)
                throw new APIllegalStatusException(Status);

            Status = MatchStatus.CANCELED;
        }
    }
}
