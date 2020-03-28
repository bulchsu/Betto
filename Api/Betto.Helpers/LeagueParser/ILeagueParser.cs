using Betto.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Helpers
{
    public interface ILeagueParser
    {
        Task<IEnumerable<LeagueEntity>> GetLeaguesAsync();
    }
}
