using AmdarisProject.Application.Common.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AmdarisProject.Presentation
{
    internal class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
            ArgumentNullException.ThrowIfNull(_jwtSettings);
            ArgumentNullException.ThrowIfNull(_jwtSettings.SecretKey);
            ArgumentNullException.ThrowIfNull(_jwtSettings.Issuer);
            ArgumentNullException.ThrowIfNull(_jwtSettings.Audience);
        }

        public string GenerateAccessToken(ClaimsIdentity identity)
        {
            var signinCredentials =
                new SigningCredentials(_jwtSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: identity.Claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.TokenLifetimeInMinutes),
                signingCredentials: signinCredentials
            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return encodedToken;
        }

        //public SecurityToken CreateSecurityToken(ClaimsIdentity identity)
        //{
        //    var tokenDescriptor = GetTokenDescriptor(identity);
        //    return new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
        //}

        //public string WriteToken(SecurityToken token) => TokenHandler.WriteToken(token);

        //private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
        //    => new()
        //    {
        //        Subject = identity,
        //        Expires = DateTime.UtcNow.AddMinutes(30),
        //        Issuer = _jwtSettings.Issuer,
        //        Audience = _jwtSettings.Audiences?[0],
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
        //    };
    }
}
