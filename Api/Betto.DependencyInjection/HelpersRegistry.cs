using Betto.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Betto.DependencyInjection
{
    internal static class HelpersRegistry
    {
        internal static IServiceCollection RegisterHelpers(this IServiceCollection services)
        {
            return services
                .AddScoped<IPasswordHasher, PasswordHasher>()
                .AddScoped<ITokenGenerator, TokenGenerator>()
                .AddScoped<IRelationCreator, RelationCreator>()
                .AddScoped<IRateCalculator, RateCalculator>();
        }
    }
}
