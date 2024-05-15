using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AmdarisProject.Presentation
{
    public class JwtSettings
    {
        public required string SecretKey { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required int TokenLifetimeInMinutes { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.ASCII.GetBytes(SecretKey));
    }
}
