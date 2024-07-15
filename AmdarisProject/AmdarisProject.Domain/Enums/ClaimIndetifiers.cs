using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Infrastructure.Identity
{
    public static class ClaimIndetifiers
    {
        public static readonly string Username = $"Username";
        public static readonly string Email = $"Email";
        public static readonly string FirstName = $"FirstName";
        public static readonly string LastName = $"LastName";
        public static readonly string PlayerId = $"{nameof(Player)}Id";
        public static readonly string ProfilePictureUri = "ProfilePictureUri";
    }
}
