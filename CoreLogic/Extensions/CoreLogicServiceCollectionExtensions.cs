using CoreLogic.Services;
using CoreLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLogic.Extensions;

public static class CoreLogicServiceCollectionExtensions
{
    public static IServiceCollection AddCoreLogicServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAdminService, AdminService>();
        return services;
    }
}