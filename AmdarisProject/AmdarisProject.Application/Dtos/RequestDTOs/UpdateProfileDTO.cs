using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AmdarisProject.Application.Dtos.RequestDTOs
{
    public class UpdateProfileDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public required string Username { get; set; }
        public required IFormFile? ProfilePicture { get; set; }
    }
}
