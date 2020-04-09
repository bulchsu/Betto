using Betto.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Betto.DependencyInjection
{
    internal static class ServicesRegistry
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services.AddScoped<IImportService, ImportService>()
                .AddScoped<ILeagueService, LeagueService>()
                .AddScoped<ITeamService, TeamService>()
                .AddScoped<IUserService, UserService>();
        }
    }
}
