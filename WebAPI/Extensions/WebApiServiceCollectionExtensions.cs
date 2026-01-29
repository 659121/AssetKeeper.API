using CoreLogic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebAPI.Services;

namespace WebAPI.Extensions;

public static class WebApiServiceCollectionExtensions
{
    public static IServiceCollection AddTokenService(this IServiceCollection services)
    {
        services.AddSingleton<ITokenService, TokenService>();
        return services;
    }
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        // Регистрируем обработчик JWT Bearer с использованием IOptions
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, null, jwtBearerOptions =>
        {
            // Настройка через IConfigureNamedOptions
        });

        // Конфигурируем параметры токена через IConfigureOptions
        services.ConfigureOptions<ConfigureJwtBearerOptions>();

        return services;
    }
}