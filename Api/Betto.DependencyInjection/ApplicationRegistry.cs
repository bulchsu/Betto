using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Betto.DependencyInjection
{
    public static class ApplicationRegistry
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .RegisterConfiguration(configuration)
                .RegisterHelpers()
                .RegisterDatabaseConnection(configuration)
                .RegisterRepositories()
                .RegisterAuthentication(configuration)
                .RegisterApiCommunication()
                .RegisterValidators()
                .RegisterServices()
                .RegisterLocalization();
        }
    }
}
