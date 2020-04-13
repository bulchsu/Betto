using Betto.DataAccessLayer.Repositories;
using Betto.Model.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Betto.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly ILeagueRepository _leagueRepository;

        public LeagueService(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        public async Task<LeagueDTO> GetLeagueByIdAsync(int leagueId, bool includeTeams, bool includeGames)
            => (LeagueDTO)(await _leagueRepository.GetLeagueByIdAsync(leagueId, includeTeams, includeGames));

        public async Task<ICollection<LeagueDTO>> GetLeaguesAsync(bool includeTeams, bool includeGames)
            => (await _leagueRepository.GetLeaguesAsync(includeTeams, includeGames))
                .Select(l => (LeagueDTO)l)
                .ToList();
    }
}
