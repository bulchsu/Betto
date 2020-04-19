using Betto.Services.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Betto.DependencyInjection
{
    internal static class ValidatorsRegistry
    {
        internal static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            return services.AddScoped<ILeagueValidator, LeagueValidator>()
                .AddScoped<IPaymentValidator, PaymentValidator>()
                .AddScoped<IUserValidator, UserValidator>()
                .AddScoped<ITicketValidator, TicketValidator>();
        }
    }
}
