using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Entities;

namespace Betto.RapidApiCommunication.Managers
{
    public interface ITeamManager
    {
        Task<IEnumerable<TeamEntity>> GetTeamsAsync(IEnumerable<int> leagueIds);
    }
}
