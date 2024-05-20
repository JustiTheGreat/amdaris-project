﻿using AmdarisProject.Application;
using AmdarisProject.Application.Extensions;
using AmdarisProject.Domain.Exceptions;
using AmdarisProject.Infrastructure.Persistance.Extensions;
using AmdarisProject.Presentation.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace AmdarisProject.Presentation.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            JwtSettings jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
                ?? throw new AmdarisProjectException("Missing JWT settings!");

            builder.Services.AddControllers();
            builder.Services
                .AddEndpointsApiExplorer()
                .AddSwaggerWithAuthorization()
                .AddAuthenticationService(jwtSettings)
                .AddInfrastructure()
                .AddApplication()
                .AddAutoMapper(typeof(AutoMapperProfileAssemblyMarker))
                .Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)))
                .Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));

        }

        private static IServiceCollection AddSwaggerWithAuthorization(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            return serviceCollection;
        }

        private static IServiceCollection AddAuthenticationService(this IServiceCollection serviceCollection, JwtSettings jwtSettings)
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
                        IssuerSigningKey = jwtSettings.GetSymmetricSecurityKey(),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    };
                    jwt.Audience = jwtSettings.Audience;
                    jwt.ClaimsIssuer = jwtSettings.Issuer;
                });
            return serviceCollection;
        }
    }
}