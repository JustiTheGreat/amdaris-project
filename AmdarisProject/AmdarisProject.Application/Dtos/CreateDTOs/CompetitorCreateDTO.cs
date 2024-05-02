using System.ComponentModel.DataAnnotations;

namespace AmdarisProject.Application.Dtos.CreateDTOs
{
    public class CompetitorCreateDTO : CreateDTO
    {
        [Required]
        public required string Name { get; set; }
    }
}
