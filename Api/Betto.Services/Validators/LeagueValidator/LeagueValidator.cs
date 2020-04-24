using System.Threading.Tasks;
using Betto.DataAccessLayer.Repositories;

namespace Betto.Services.Validators
{
    public class LeagueValidator : ILeagueValidator
    {
        private readonly ILeagueRepository _leagueRepository;

        public LeagueValidator(ILeagueRepository leagueRepository)
        {
            _leagueRepository = leagueRepository;
        }

        public async Task<bool> CheckDoesTheLeagueExistAsync(int leagueId) =>
            await _leagueRepository.GetLeagueByIdAsync(leagueId, false, false) != null;
    }
}
