using Betto.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Models;

namespace Betto.Services
{
    public interface ITeamService
    {
        Task<RequestResponseModel<ICollection<TeamViewModel>>> GetLeagueTeamsAsync(int leagueId);
        Task<RequestResponseModel<TeamViewModel>> GetTeamByIdAsync(int teamId, bool includeVenue);
    }
}
