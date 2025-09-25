using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace DataAccess;
public static class Extensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollecton, string connectionString)
    {
        serviceCollecton.AddScoped<IAuthRepository, AuthRepository>();
        serviceCollecton.AddScoped<IUserRepository, UserRepository>();
        serviceCollecton.AddDbContext<AppContext>(x =>
        {
            x.UseSqlite(connectionString);
        });
        return serviceCollecton;
    }
}
