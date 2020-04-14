using Betto.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Services
{
    public interface ITeamService
    {
        Task<ICollection<TeamViewModel>> GetLeagueTeamsAsync(int leagueId);
        Task<TeamViewModel> GetTeamByIdAsync(int id, bool includeVenue);
    }
}
