using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Betto.DependencyInjection
{
    public static class ApplicationRegistry
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            return services.RegisterConfiguration(configuration)
                .RegisterDatabaseConnection(configuration)
                .RegisterRepositories()
                .RegisterServices()
                .RegisterHelpers()
                .RegisterAuthentication(configuration)
                .RegisterLocalization();
        }
    }
}
