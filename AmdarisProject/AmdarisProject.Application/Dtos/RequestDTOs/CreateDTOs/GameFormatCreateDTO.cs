using AmdarisProject.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs
{
    public class GameFormatCreateDTO : CreateDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Game type is required")]
        public required GameType GameType { get; set; }
        [Required(ErrorMessage = "Competitor type is required")]
        public required CompetitorType CompetitorType { get; set; }
        public required uint? TeamSize { get; set; }
        public required uint? WinAt { get; set; }
        public required ulong? DurationInMinutes { get; set; }
    }
}
