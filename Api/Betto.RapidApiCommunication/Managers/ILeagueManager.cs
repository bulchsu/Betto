using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Entities;

namespace Betto.RapidApiCommunication.Managers
{
    public interface ILeagueManager
    {
        Task<IEnumerable<LeagueEntity>> GetLeaguesAsync(ICollection<int> leaguesToImportIds);
    }
}
