using Betto.Model.Entities;
using Betto.RapidApiCommunication;
using Betto.RapidApiCommunication.Managers;
using Betto.RapidApiCommunication.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Betto.DependencyInjection
{
    internal static class ApiCommunicationRegistry
    {
        internal static IServiceCollection RegisterApiCommunication(this IServiceCollection services)
        {

            return services
                .AddScoped<ApiClient>()
                .AddScoped(typeof(IParser<GameEntity>), typeof(GameParser))
                .AddScoped(typeof(IParser<TeamEntity>), typeof(TeamParser))
                .AddScoped(typeof(IParser<LeagueEntity>), typeof(LeagueParser))
                .AddScoped<IGameManager, GameManager>()
                .AddScoped<ITeamManager, TeamManager>()
                .AddScoped<ILeagueManager, LeagueManager>();
        }
    }
}
