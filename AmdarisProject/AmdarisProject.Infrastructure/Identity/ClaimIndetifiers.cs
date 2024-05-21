using AmdarisProject.Application.Dtos.RequestDTOs;
using AmdarisProject.Domain.Models.CompetitorModels;

namespace AmdarisProject.Infrastructure.Identity
{
    public static class ClaimIndetifiers
    {
        public static readonly string FirstName = $"FirstName";
        public static readonly string LastName = $"LastName";
        public static readonly string PlayerId = $"{nameof(Player)}Id";
    }
}
