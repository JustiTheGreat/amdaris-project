﻿using System.ComponentModel.DataAnnotations;

namespace AmdarisProject.Application.Dtos.CreateDTOs
{
    public class CompetitionCreateDTO : CreateDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Location { get; set; }
        [Required]
        public required DateTime StartTime { get; set; }
        [Required]
        public required Guid GameFormat { get; set; }
        public ulong? BreakInMinutes { get; set; }
    }
}