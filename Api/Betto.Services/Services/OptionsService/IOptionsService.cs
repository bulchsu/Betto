using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Services
{
    public interface IOptionsService
    {
        Task ImportInitialDataAsync();
        Task ImportNextLeaguesAsync(int leaguesAmount);
        Task SetBetRatesForAllLeaguesAsync();
        Task SetBetRatesForAdditionalLeaguesAsync(int amount);
    }
}
