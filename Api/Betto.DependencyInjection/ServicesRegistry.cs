using Betto.Services;
using Betto.Services;
using Betto.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Betto.DependencyInjection
{
    internal static class ServicesRegistry
    {
        internal static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services.AddScoped<IOptionsService, OptionsService>()
                .AddScoped<ILeagueService, LeagueService>()
                .AddScoped<ITeamService, TeamService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IGameService, GameService>()
                .AddScoped<ITicketService, TicketService>()
                .AddScoped<IPaymentService, PaymentService>();
        }
    }
}
