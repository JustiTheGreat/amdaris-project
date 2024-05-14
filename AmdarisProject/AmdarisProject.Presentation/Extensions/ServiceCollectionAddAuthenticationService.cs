using AmdarisProject.Domain.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AmdarisProject.Presentation.Extensions
{
    public static class ServiceCollectionAddAuthenticationService
    {
        public static IServiceCollection AddAuthenticationService(this IServiceCollection serviceCollection, JwtSettings jwtSettings)
        {
            serviceCollection
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwt =>
                {
                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey
                            ?? throw new AmdarisProjectException("Missing SigningKey setting!"))),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudiences = jwtSettings.Audiences,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    };
                    jwt.Audience = jwtSettings.Audiences?[0] ?? throw new AmdarisProjectException("Missing SigningKey setting!");
                    jwt.ClaimsIssuer = jwtSettings.Issuer;
                });
            return serviceCollection;
        }
    }
}
