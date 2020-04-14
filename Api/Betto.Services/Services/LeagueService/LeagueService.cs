using Betto.DataAccessLayer.Repositories;
using Betto.Model.ViewModels;
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

        public async Task<LeagueViewModel> GetLeagueByIdAsync(int leagueId, bool includeTeams, bool includeGames)
            => (LeagueViewModel)(await _leagueRepository.GetLeagueByIdAsync(leagueId, includeTeams, includeGames));

        public async Task<ICollection<LeagueViewModel>> GetLeaguesAsync(bool includeTeams, bool includeGames)
            => (await _leagueRepository.GetLeaguesAsync(includeTeams, includeGames))
                .Select(l => (LeagueViewModel)l)
                .ToList();
    }
}
