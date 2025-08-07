using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace DataAccess;
public static class Extensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection serviceCollecton, string connectionString)
    {
        serviceCollecton.AddScoped<IAuthRepository, AuthRepository>();
        serviceCollecton.AddDbContext<AppContext>(x =>
        {
            x.UseSqlite(connectionString);
        });
        return serviceCollecton;
    }
}
