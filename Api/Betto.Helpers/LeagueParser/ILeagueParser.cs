using Betto.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Helpers.LeagueParser
{
    public interface ILeagueParser
    {
        Task<IEnumerable<LeagueEntity>> GetLeaguesAsync();
    }
}
