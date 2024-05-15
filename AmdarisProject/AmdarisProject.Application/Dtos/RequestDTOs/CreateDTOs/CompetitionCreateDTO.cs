using System.ComponentModel.DataAnnotations;

namespace AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs
{
    public class CompetitionCreateDTO : CreateDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Location is required")]
        public required string Location { get; set; }
        [Required(ErrorMessage = "Start time is required")]
        public required DateTime StartTime { get; set; }
        [Required(ErrorMessage = "Game format is required")]
        public required Guid GameFormat { get; set; }
        public ulong? BreakInMinutes { get; set; }
    }
}
