using System.Security.Claims;

namespace AmdarisProject.Application.Abstractions
{
    public interface ITokenService
    {
        string GenerateAccessToken(ClaimsIdentity identity);
    }
}
