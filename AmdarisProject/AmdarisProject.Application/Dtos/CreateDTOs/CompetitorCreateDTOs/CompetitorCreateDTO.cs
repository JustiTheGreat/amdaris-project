using System.ComponentModel.DataAnnotations;

namespace AmdarisProject.Application.Dtos.CreateDTOs.CompetitorCreateDTOs
{
    public class CompetitorCreateDTO : CreateDTO
    {
        [Required]
        public required string Name { get; set; }
    }
}
