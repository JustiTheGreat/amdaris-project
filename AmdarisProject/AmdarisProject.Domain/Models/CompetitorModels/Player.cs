﻿namespace AmdarisProject.Domain.Models.CompetitorModels
{
    public class Player : Competitor
    {
        public virtual required List<Point> Points { get; set; } = [];
        public virtual required List<Team> Teams { get; set; } = [];
        public required string? ProfilePictureUri { get; set; }

        public override bool IsOrContainsCompetitor(Guid competitorId)
            => Id.Equals(competitorId);
    }
}
