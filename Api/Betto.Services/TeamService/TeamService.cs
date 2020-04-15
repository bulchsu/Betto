using Betto.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Betto.DataAccessLayer.Repositories;
using Betto.Helpers.Extensions;
using Betto.Model.Models;
using Betto.Resources.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Betto.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ILeagueRepository _leagueRepository;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public TeamService(ITeamRepository teamRepository,
            ILeagueRepository leagueRepository,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _teamRepository = teamRepository;
            _leagueRepository = leagueRepository;
            _localizer = localizer;
        }

        public async Task<RequestResponseModel<ICollection<TeamViewModel>>> GetLeagueTeamsAsync(int leagueId)
        {
            var doesLeagueExist = await CheckDoesLeagueExistAsync(leagueId);

            if (!doesLeagueExist)
            {
                return new RequestResponseModel<ICollection<TeamViewModel>>(StatusCodes.Status404NotFound,
                    new List<ErrorViewModel>
                    {
                        ErrorViewModel.Factory.NewErrorFromMessage(_localizer["LeagueNotFoundErrorMessage"]
                            .Value)
                    },
                    null);
            }

            var leagueTeams = (await _teamRepository.GetLeagueTeamsAsync(leagueId))
                .Select(t => (TeamViewModel)t)
                .ToList()
                .GetEmptyIfNull();

            return new RequestResponseModel<ICollection<TeamViewModel>>(StatusCodes.Status200OK, 
                Enumerable.Empty<ErrorViewModel>(), 
                leagueTeams);
        }

        private async Task<bool> CheckDoesLeagueExistAsync(int leagueId) =>
            await _leagueRepository.GetLeagueByIdAsync(leagueId, false, false) != null;
    }
}
