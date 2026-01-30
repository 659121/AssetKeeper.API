using CoreLogic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoreLogic.Extensions;

public static class InventoryServiceCollectionExtensions
{
    public static IServiceCollection AddInventoryServices(this IServiceCollection services)
    {
        // Регистрация сервиса инвентаризации
        services.AddScoped<IInventoryService, InventoryService>();
        return services;
    }
}