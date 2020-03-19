using Betto.DataAccessLayer.Repositories.TeamRepository;
using Betto.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Betto.Services.Services.TeamService
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            this._teamRepository = teamRepository;
        }

        public async Task<IEnumerable<TeamDTO>> GetLeagueTeamsAsync(int leagueId)
            => (await _teamRepository.GetLeagueTeamsAsync(leagueId))
            .Select(t => (TeamDTO)t);

        public async Task<TeamDTO> GetTeamByIdAsync(int id, bool includeVenue)
            => (TeamDTO)(await _teamRepository.GetTeamByIdAsync(id, includeVenue));
    }
}
