using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Entities;

namespace Betto.RapidApiCommunication.Managers
{
    public interface IGameManager
    {
        Task<IEnumerable<GameEntity>> GetGamesAsync(IEnumerable<int> leagueIds);
    }
}
