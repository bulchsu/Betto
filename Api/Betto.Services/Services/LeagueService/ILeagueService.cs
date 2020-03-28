using Betto.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Services.Services
{
    public interface ILeagueService
    {
        Task<LeagueDTO> GetLeagueByIdAsync(int leagueId);
        Task<IEnumerable<LeagueDTO>> GetLeaguesAsync();
    }
}
