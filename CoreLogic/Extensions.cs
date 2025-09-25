using CoreLogic.Models.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace CoreLogic;

public static class Extensions
{
    public static IServiceCollection AddCoreLogic(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthService, AuthService>();
        serviceCollection.AddScoped<IAdminService, AdminService>();
        return serviceCollection;
    }

    // TODO перевести настройки на ioptions
    public static IServiceCollection AddTokenService(this IServiceCollection serviceCollection, JwtSettings jwtSettings)
    {
        if (string.IsNullOrWhiteSpace(jwtSettings.Secret))
            throw new ArgumentException("JWT Secret is required");

        serviceCollection.AddSingleton(jwtSettings);
        serviceCollection.AddSingleton<ITokenService, TokenService>();
        return serviceCollection;
    }

    public static IServiceCollection AddAuth(this IServiceCollection serviceCollection, JwtSettings jwtSettings)
    {
        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role
            };
        });

        // TODO настроить роли
        serviceCollection.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminRole", policy => policy.RequireClaim("roles", "Admin"))
            .AddPolicy("RequireUserRole", policy => policy.RequireClaim("roles", "User"));

        return serviceCollection;
    }
}