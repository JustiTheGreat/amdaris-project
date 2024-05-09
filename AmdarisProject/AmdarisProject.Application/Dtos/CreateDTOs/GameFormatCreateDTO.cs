using AmdarisProject.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AmdarisProject.Application.Dtos.CreateDTOs
{
    public class GameFormatCreateDTO : CreateDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required GameType GameType { get; set; }
        [Required]
        public required CompetitorType CompetitorType { get; set; }
        public uint? TeamSize { get; set; }
        public uint? WinAt { get; set; }
        public ulong? DurationInMinutes { get; set; }
    }
}
