using Betto.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Services.Services.TeamService
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamDTO>> GetLeagueTeamsAsync(int leagueId);
        Task<TeamDTO> GetTeamByIdAsync(int id, bool includeVenue);
    }
}
