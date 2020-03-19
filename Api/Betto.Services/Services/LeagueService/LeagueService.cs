using Betto.DataAccessLayer.LeagueRepository.Repositories;
using Betto.Model.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Betto.Services.Services.LeagueService
{
    public class LeagueService : ILeagueService
    {
        private readonly ILeagueRepository _leagueRepository;

        public LeagueService(ILeagueRepository leagueRepository)
        {
            this._leagueRepository = leagueRepository;
        }

        public async Task<LeagueDTO> GetLeagueByIdAsync(int leagueId)
            => (LeagueDTO)(await _leagueRepository.GetLeagueByIdAsync(leagueId));

        public async Task<IEnumerable<LeagueDTO>> GetLeaguesAsync()
            => (await _leagueRepository.GetLeaguesAsync()).Select(l => (LeagueDTO)l);
    }
}
