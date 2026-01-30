using CoreLogic.Interfaces;
using DataAccess.DatabaseContexts;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;
public static class Extensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollecton, string connectionString)
    {
        serviceCollecton.AddScoped<IAuthRepository, AuthRepository>();
        serviceCollecton.AddScoped<IUserRepository, UserRepository>();
        serviceCollecton.AddDbContext<AuthContext>(x =>
        {
            x.UseSqlite(connectionString);
        });
        return serviceCollecton;
    }

    public static IServiceCollection AddStockDataAccess(this IServiceCollection serviceCollecton, string connectionString)
    {
        // Регистрация репозиториев инвентаризации
        serviceCollecton.AddScoped<IDeviceRepository, DeviceRepository>();
        serviceCollecton.AddScoped<IDepartmentRepository, DepartmentRepository>();
        serviceCollecton.AddScoped<IDeviceStatusRepository, DeviceStatusRepository>();
        serviceCollecton.AddScoped<IMovementReasonRepository, MovementReasonRepository>();
        serviceCollecton.AddScoped<IDeviceMovementRepository, DeviceMovementRepository>();
        serviceCollecton.AddDbContext<InventoryDbContext>(x =>
        {
            x.UseSqlite(connectionString);
        });
        return serviceCollecton;
    }
}