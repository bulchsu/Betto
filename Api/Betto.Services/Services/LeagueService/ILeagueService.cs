using Betto.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Models;

namespace Betto.Services
{
    public interface ILeagueService
    {
        Task<LeagueDTO> GetLeagueByIdAsync(int leagueId, bool includeTeams, bool includeGames);
        Task<IEnumerable<LeagueDTO>> GetLeaguesAsync();
        Task<LeagueTable> GetLeagueTableAsync(int leagueId);
    }
}
