using Betto.DataAccessLayer.Repositories;
using Betto.DataAccessLayer.Repositories.PaymentRepository;
using Betto.DataAccessLayer.Repositories.RatesRepository;
using Microsoft.Extensions.DependencyInjection;

namespace Betto.DependencyInjection
{
    internal static class RepositoriesRegistry
    {
        internal static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IUserRepository, UserRepository>()
                .AddScoped<ILeagueRepository, LeagueRepository>()
                .AddScoped<ITeamRepository, TeamRepository>()
                .AddScoped<IGameRepository, GameRepository>()
                .AddScoped<IRatesRepository, RatesRepository>()
                .AddScoped<ITicketRepository, TicketRepository>()
                .AddScoped<IPaymentRepository, PaymentRepository>();
        }
    }
}
