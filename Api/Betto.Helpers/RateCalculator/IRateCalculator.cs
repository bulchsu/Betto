using System.Collections.Generic;
using Betto.Model.Entities;

namespace Betto.Helpers
{
    public interface IRateCalculator
    {
        ICollection<BetRatesEntity> GetLeagueGamesRates(LeagueEntity league);
    }
}