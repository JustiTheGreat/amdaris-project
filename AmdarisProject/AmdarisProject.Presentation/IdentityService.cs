using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AmdarisProject.Presentation
{
    public class IdentityService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly byte[] _key;

        public IdentityService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
            ArgumentNullException.ThrowIfNull(_jwtSettings);
            ArgumentNullException.ThrowIfNull(_jwtSettings.SigningKey);
            ArgumentNullException.ThrowIfNull(_jwtSettings.Issuer);
            ArgumentNullException.ThrowIfNull(_jwtSettings.Audiences);
            ArgumentNullException.ThrowIfNull(_jwtSettings.Audiences[0]);
            _key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
        }

        private static JwtSecurityTokenHandler TokenHandler => new();

        public SecurityToken CreateSecurityToken(ClaimsIdentity identity)
        {
            var tokenDescriptor = GetTokenDescriptor(identity);
            return TokenHandler.CreateToken(tokenDescriptor);
        }

        public string WriteToken(SecurityToken token) => TokenHandler.WriteToken(token);

        private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
            => new()
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audiences?[0],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
            };
    }
}
