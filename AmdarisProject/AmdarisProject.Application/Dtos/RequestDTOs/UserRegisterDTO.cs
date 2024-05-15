using AmdarisProject.Presentation;
using System.ComponentModel.DataAnnotations;

namespace AmdarisProject.Application.Dtos.RequestDTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public required string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public required string LastName { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public required UserRole Role { get; set; }
    }
}
