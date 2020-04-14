using Betto.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Betto.DataAccessLayer.Repositories;

namespace Betto.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<ICollection<TeamViewModel>> GetLeagueTeamsAsync(int leagueId)
            => (await _teamRepository.GetLeagueTeamsAsync(leagueId))
            .Select(t => (TeamViewModel)t)
            .ToList();

        public async Task<TeamViewModel> GetTeamByIdAsync(int id, bool includeVenue)
            => (TeamViewModel)(await _teamRepository.GetTeamByIdAsync(id, includeVenue));
    }
}
