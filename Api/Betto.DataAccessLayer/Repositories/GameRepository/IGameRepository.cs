using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Entities;

namespace Betto.DataAccessLayer.Repositories
{
    public interface IGameRepository : IBaseRepository
    {
        Task<ICollection<GameEntity>> GetLeagueGamesAsync(int leagueId);
    }
}
