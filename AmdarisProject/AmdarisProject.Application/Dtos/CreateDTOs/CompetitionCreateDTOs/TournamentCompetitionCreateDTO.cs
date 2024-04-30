using System.ComponentModel.DataAnnotations;

namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitionCreateDTOs
{
    public class TournamentCompetitionCreateDTO : CompetitionCreateDTO
    {
        [Required]
        public required ushort StageLevel { get; set; }
    }
}
