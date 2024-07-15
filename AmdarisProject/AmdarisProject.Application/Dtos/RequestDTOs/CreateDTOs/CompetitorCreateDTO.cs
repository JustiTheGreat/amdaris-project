using System.ComponentModel.DataAnnotations;

namespace AmdarisProject.Application.Dtos.RequestDTOs.CreateDTOs
{
    public class CompetitorCreateDTO : CreateDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }
        public string? ProfilePictureUri { get; set; }
    }
}
