using Betto.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Services
{
    public interface ITeamService
    {
        Task<ICollection<TeamDTO>> GetLeagueTeamsAsync(int leagueId);
        Task<TeamDTO> GetTeamByIdAsync(int id, bool includeVenue);
    }
}
