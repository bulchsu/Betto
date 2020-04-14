using Betto.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Models;

namespace Betto.Services
{
    public interface ILeagueService
    {
        Task<RequestResponse<LeagueViewModel>> GetLeagueByIdAsync(int leagueId, bool includeTeams, bool includeGames);
        Task<RequestResponse<ICollection<LeagueViewModel>>> GetLeaguesAsync(bool includeTeams, bool includeGames);
    }
}
