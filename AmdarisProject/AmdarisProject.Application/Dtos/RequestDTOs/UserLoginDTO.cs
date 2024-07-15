using System.ComponentModel.DataAnnotations;

namespace AmdarisProject.Application.Dtos.RequestDTOs
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }
}
