﻿using AmdarisProject.models.competitor;
using AmdarisProject.utils;
using AmdarisProject.utils.enums;
using AmdarisProject.utils.Exceptions;

namespace AmdarisProject.models.competition
{
    public class Competition : ICloneable
    {
        private static int instances = 0;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public GameType GameType { get; set; }
        public CompetitorType CompetitorType { get; set; }
        public CompetitionStatus Status { get; set; }
        public IEnumerable<Competitor> Competitors { get; set; }
        public IEnumerable<Match> Matches { get; set; }

        protected Competition(string name, string location, DateTime startTime, GameType gameType, CompetitorType competitorType)
        {
            Id = ++instances;
            Name = name;
            Location = location;
            StartTime = startTime;
            GameType = gameType;
            CompetitorType = competitorType;
            Status = CompetitionStatus.ORGANIZING;
            Competitors = [];
            Matches = [];
        }

        public void Register(Competitor competitor)
        {
            if (Status != CompetitionStatus.ORGANIZING)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(Register), Status.ToString()));

            Competitors = Competitors.Append(competitor);
            Console.WriteLine($"Competitor {competitor.Name} have stopped!");
        }

        public void StopRegistrations()
        {
            if (Status != CompetitionStatus.ORGANIZING)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(StopRegistrations), Status.ToString()));

            Status = CompetitionStatus.NOT_STARTED;
            Console.WriteLine($"Registrations for competition {Name} have stopped!");
        }

        public void Start()
        {
            if (Status != CompetitionStatus.NOT_STARTED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(Start), Status.ToString()));

            Status = CompetitionStatus.STARTED;
            Console.WriteLine($"Competition {Name} started!");
        }

        public int GetPoints(Competitor competitor)
        {
            if (Status == CompetitionStatus.ORGANIZING || Status == CompetitionStatus.NOT_STARTED || Status == CompetitionStatus.CANCELED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(Start), Status.ToString()));

            return Matches.Where(match => match.CompetitorOne.Equals(competitor) || match.CompetitorTwo.Equals(competitor))
                .Select(match => match.CompetitorOne.Equals(competitor) ? match.GetPointsCompetitorOne() : match.GetPointsCompetitorTwo())
                .Aggregate((points1, points2) => points1 + points2);
        }

        public int GetPoints(Match match, Competitor competitor)
        {
            if (Status == CompetitionStatus.ORGANIZING || Status == CompetitionStatus.NOT_STARTED || Status == CompetitionStatus.CANCELED)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(GetPoints), Status.ToString()));

            if (match.Status == MatchStatus.NOT_STARTED || match.Status == MatchStatus.CANCELED
                || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_ONE || match.Status == MatchStatus.SPECIAL_WIN_COMPETITOR_TWO)
                throw new IllegalStatusException(MessageFormatter.Format(nameof(Competition), nameof(GetPoints), match.Status.ToString()));

            return competitor.GetPoints(match);
        }

        protected Competition(int id, string name, string location, DateTime startTime, GameType gameType, CompetitorType competitorType,
            CompetitionStatus status, IEnumerable<Competitor> competitors, IEnumerable<Match> matches)
        {
            Id = id;
            Name = name;
            Location = location;
            StartTime = startTime;
            GameType = gameType;
            Status = status;
            CompetitorType = competitorType;
            Competitors = competitors;
            Matches = matches;
        }

        public virtual object Clone()
        {
            return new Competition(
                Id,
                Name,
                Location,
                StartTime,
                GameType,
                CompetitorType,
                Status,
                Competitors,
                Matches
                );
        }
    }
}
