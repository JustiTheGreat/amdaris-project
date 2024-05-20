using AmdarisProject.Domain.Enums;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Domain.Models.CompetitionModels
{
    public abstract class Competition : Model
    {
        public required string Name { get; set; }
        public required string Location { get; set; }
        public required DateTime StartTime { get; set; }
        public required CompetitionStatus Status { get; set; }
        public required ulong? BreakInMinutes { get; set; }
        public virtual required GameFormat GameFormat { get; set; }
        public virtual required List<Competitor> Competitors { get; set; } = [];
        public virtual required List<Match> Matches { get; set; } = [];

        public abstract bool CantContinue();

        public abstract void CheckCompetitionCompetitorNumber();

        public abstract bool ShouldCreateMatches();

        public void StopRegistrations()
        {
            if (Status is not CompetitionStatus.ORGANIZING)
                throw new APIllegalStatusException(Status);

            CheckCompetitionCompetitorNumber();

            Status = CompetitionStatus.NOT_STARTED;
        }

        public void Start()
        {
            if (Status is not CompetitionStatus.NOT_STARTED)
                throw new APIllegalStatusException(Status);

            Status = CompetitionStatus.STARTED;
        }

        public void End()
        {
            if (Status is not CompetitionStatus.STARTED)
                throw new APIllegalStatusException(Status);

            if (!AllMatchesOfCompetitonAreDone())
                throw new AmdarisProjectException($"Competition {Id} still has unfinished matches!");

            Status = CompetitionStatus.FINISHED;
        }

        public void Cancel()
        {
            if (Status is not CompetitionStatus.ORGANIZING
                && Status is not CompetitionStatus.NOT_STARTED
                && Status is not CompetitionStatus.STARTED)
                throw new APIllegalStatusException(Status);

            if (AMatchIsBeignPlayed())
                throw new AmdarisProjectException($"A match is from competition {Name} is being played!");

            Status = CompetitionStatus.FINISHED;

            Matches.ForEach(match => match.Cancel());
        }

        public bool AllMatchesOfCompetitonAreDone()
            => Matches.All(match => match.Status == MatchStatus.FINISHED
                || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_ONE
                || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_TWO
                || match.Status == MatchStatus.CANCELED);

        public bool ContainsCompetitor(Guid competitorId)
            => Competitors.Any(competitor => competitor.Id.Equals(competitorId));

        public bool ContainsPlayer(Guid playerId)
            => Competitors.Any(competitor => competitor.IsOrContainsCompetitor(playerId));

        public int GetCompetitorPoints(Guid competitorId)
            => Matches
            .Where(match => match.CompetitorOnePoints is not null
                && match.CompetitorTwoPoints is not null
                && (match.CompetitorOne.Id.Equals(competitorId) || match.CompetitorTwo.Id.Equals(competitorId)))
            .Select(match => match.CompetitorOne.Id.Equals(competitorId)
                ? (int)match.CompetitorOnePoints! : (int)match.CompetitorTwoPoints!)
            .Aggregate(0, (result, point) => result + point);

        public int GetCompetitorWins(Guid competitorId)
            => Matches.Count(match => match.Winner != null && match.Winner.Id.Equals(competitorId));

        public bool AMatchIsBeignPlayed()
            => Matches.Any(match => match.Status is MatchStatus.STARTED);

        public void AddCompetitor(Competitor competitor)
        {
            if (Status is not CompetitionStatus.ORGANIZING)
                throw new APIllegalStatusException(Status);

            if (competitor is Player && GameFormat.CompetitorType is not CompetitorType.PLAYER
                || competitor is Team && GameFormat.CompetitorType is not CompetitorType.TEAM)
                throw new AmdarisProjectException($"Tried to add {competitor.GetType().Name} " +
                    $"to competition with {GameFormat.CompetitorType} competitor type!");

            if (ContainsCompetitor(Id))
                throw new AmdarisProjectException($"Competitor {competitor.Id} is already registered to competition {Id}!");

            if (competitor is Team team)
            {
                if (team.ContainsAPlayerPartOfAnotherTeamFromCompetition(this))
                    throw new AmdarisProjectException($"Team {competitor.Id} is contains a player part of another team from competition {Id}!");

                if (!team.HasTheRequiredNumberOfActivePlayers((uint)GameFormat.TeamSize!))
                    throw new AmdarisProjectException($"Team {competitor.Id} doesn't have the required number of active competitors!");
            }

            Competitors.Add(competitor);
        }

        public void RemoveCompetitor(Competitor competitor)
        {
            if (Status is not CompetitionStatus.ORGANIZING)
                throw new APIllegalStatusException(Status);

            if (competitor is Player && GameFormat.CompetitorType is not CompetitorType.PLAYER
                || competitor is Team && GameFormat.CompetitorType is not CompetitorType.TEAM)
                throw new AmdarisProjectException($"Tried to remove {competitor.GetType().Name} " +
                    $"from competition with {GameFormat.CompetitorType} competitor type!");

            if (!ContainsCompetitor(competitor.Id))
                throw new AmdarisProjectException($"Competitor {competitor.Id} is not registered to competition {Id}!");

            Competitors = Competitors.Where(c => !c.Id.Equals(competitor.Id)).ToList();
        }

        public Match? GetMatchByTheTwoCompetitors(Guid competitorId1, Guid competitorId2)
            => Matches.FirstOrDefault(match =>
                match.CompetitorOne.Id.Equals(competitorId1) && match.CompetitorTwo.Id.Equals(competitorId2)
                || match.CompetitorOne.Id.Equals(competitorId2) && match.CompetitorTwo.Id.Equals(competitorId1));

        public bool DurationAndBreakTimesAreMatching() => BreakInMinutes is null ^ GameFormat.DurationInMinutes is null;
    }
}
