using Microsoft.Extensions.DependencyInjection;

namespace Betto.DependencyInjection
{
    internal static class LocalizationRegistry
    {
        internal static IServiceCollection RegisterLocalization(this IServiceCollection services) =>
            services.AddLocalization(o => o.ResourcesPath = "Resources");
    }
}
