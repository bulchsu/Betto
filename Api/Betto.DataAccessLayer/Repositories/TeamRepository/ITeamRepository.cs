using Betto.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories
{
    public interface ITeamRepository : IBaseRepository
    {
        Task<IEnumerable<TeamEntity>> GetLeagueTeamsAsync(int leagueId);
        Task<TeamEntity> GetTeamByIdAsync(int id, bool includeVenue);
    }
}
