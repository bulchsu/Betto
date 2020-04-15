using Betto.DataAccessLayer.Repositories;
using Betto.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.Helpers.Extensions;
using Betto.Model.Models;
using Betto.Resources.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Betto.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public LeagueService(ILeagueRepository leagueRepository,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _leagueRepository = leagueRepository;
            _localizer = localizer;
        }

        public async Task<RequestResponseModel<LeagueViewModel>> GetLeagueByIdAsync(int leagueId, bool includeTeams, bool includeGames)
        {
            var league = (LeagueViewModel) await _leagueRepository.GetLeagueByIdAsync(leagueId, includeTeams, includeGames);

            if (league == null)
            {
                return new RequestResponseModel<LeagueViewModel>(StatusCodes.Status404NotFound,
                    new List<ErrorViewModel>
                    {
                        new ErrorViewModel
                        {
                            Message = _localizer["LeagueNotFoundErrorMessage"]
                                .Value
                        }
                    },
                    null);
            }

            return new RequestResponseModel<LeagueViewModel>(StatusCodes.Status200OK, 
                Enumerable.Empty<ErrorViewModel>(), 
                league);
        }

        public async Task<RequestResponseModel<ICollection<LeagueViewModel>>> GetLeaguesAsync(bool includeTeams, bool includeGames)
        {
            var leagues = (await _leagueRepository.GetLeaguesAsync(includeTeams, includeGames))
                .Select(l => (LeagueViewModel)l)
                .ToList()
                .GetEmptyIfNull();

            return new RequestResponseModel<ICollection<LeagueViewModel>>(StatusCodes.Status200OK, 
                Enumerable.Empty<ErrorViewModel>(), 
                leagues);
        }
    }
}
