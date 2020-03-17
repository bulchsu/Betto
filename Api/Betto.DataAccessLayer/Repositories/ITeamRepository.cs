using Betto.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories
{
    public interface ITeamRepository
    {
        Task AddLeagueTeamsAsync(IEnumerable<TeamEntity> teams);
    }
}
