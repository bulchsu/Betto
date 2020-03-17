using Betto.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories
{
    public interface ILeagueRepository : IBaseRepository
    {
        Task AddLeaguesAsync(IEnumerable<LeagueEntity> leagues);
        void Clear();
    }
}
