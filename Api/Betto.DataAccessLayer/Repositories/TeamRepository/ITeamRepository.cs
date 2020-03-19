using Betto.DataAccessLayer.Repositories.BaseRepository;
using Betto.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories.TeamRepository
{
    public interface ITeamRepository : IBaseRepository
    {
        Task<IEnumerable<TeamEntity>> GetLeagueTeamsAsync(int leagueId);
        Task<TeamEntity> GetTeamByIdAsync(int id, bool includeVenue);
    }
}
