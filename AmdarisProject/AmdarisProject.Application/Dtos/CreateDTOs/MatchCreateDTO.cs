﻿using AmdarisProject.Domain.Enums;

namespace AmdarisProject.Application.Dtos.CreateDTOs
{
    public class MatchCreateDTO : CreateDTO
    {
        public required string Location { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public required MatchStatus Status { get; set; }
        public required Guid CompetitorOne { get; set; }
        public required Guid CompetitorTwo { get; set; }
        public required Guid Competition { get; set; }
        public Guid? Stage { get; set; }
        public List<Guid> Points { get; set; } = [];
    }
}