using Betto.Model.DTO;
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

        public async Task<ICollection<TeamDTO>> GetLeagueTeamsAsync(int leagueId)
            => (await _teamRepository.GetLeagueTeamsAsync(leagueId))
            .Select(t => (TeamDTO)t)
            .ToList();

        public async Task<TeamDTO> GetTeamByIdAsync(int id, bool includeVenue)
            => (TeamDTO)(await _teamRepository.GetTeamByIdAsync(id, includeVenue));
    }
}
