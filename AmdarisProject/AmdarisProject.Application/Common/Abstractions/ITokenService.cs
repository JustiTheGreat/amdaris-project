using System.Security.Claims;

namespace AmdarisProject.Application.Common.Abstractions
{
    public interface ITokenService
    {
        string GenerateAccessToken(ClaimsIdentity identity);
    }
}
