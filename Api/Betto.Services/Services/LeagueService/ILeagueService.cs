using Betto.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Services
{
    public interface ILeagueService
    {
        Task<LeagueDTO> GetLeagueByIdAsync(int leagueId, bool includeTeams, bool includeGames);
        Task<IEnumerable<LeagueDTO>> GetLeaguesAsync(bool includeTeams, bool includeGames);
    }
}
