using Betto.DataAccessLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Betto.DependencyInjection
{
    internal static class RepositoriesRegistry
    {
        internal static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ILeagueRepository, LeagueRepository>()
                .AddScoped<ITeamRepository, TeamRepository>();
        }
    }
}
