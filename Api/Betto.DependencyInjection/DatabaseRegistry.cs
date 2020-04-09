using Betto.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Betto.DependencyInjection
{
    internal static class DatabaseRegistry
    {
        internal static IServiceCollection RegisterDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DevelopmentConnectionString");
            return services.AddDbContext<BettoDbContext>(options => options.UseSqlServer(connectionString,
                o => o.MigrationsAssembly("Betto.DataAccessLayer")));
        }
    }
}
