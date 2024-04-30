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
        [Required]
        public ushort? TeamSize { get; set; }
        [Required]
        public uint? WinAt { get; set; }
        [Required]
        public ulong? DurationInSeconds { get; set; }
    }
}
