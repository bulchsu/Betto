using Betto.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Services
{
    public interface ILeagueService
    {
        Task<LeagueViewModel> GetLeagueByIdAsync(int leagueId, bool includeTeams, bool includeGames);
        Task<ICollection<LeagueViewModel>> GetLeaguesAsync(bool includeTeams, bool includeGames);
    }
}
