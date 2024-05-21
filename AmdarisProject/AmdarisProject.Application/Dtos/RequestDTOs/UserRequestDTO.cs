using System.ComponentModel.DataAnnotations;

namespace AmdarisProject.Application.Dtos.RequestDTOs
{
    public class UserRequestDTO
    {
        public Guid? PlayerId { get; set; }
        [Required]
        public required Guid OtherId { get; set; }
    }
}
