using Betto.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Betto.DependencyInjection
{
    internal static class ConfigurationRegistry
    {
        internal static IServiceCollection RegisterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            return services.Configure<RapidApiConfiguration>(configuration.GetSection(nameof(RapidApiConfiguration)))
                .Configure<LoggingConfiguration>(configuration.GetSection(nameof(LoggingConfiguration)))
                .Configure<ApplicationMainConfiguration>(configuration.GetSection(nameof(ApplicationMainConfiguration)));
        }
    }
}
