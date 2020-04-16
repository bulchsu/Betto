using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.DataAccessLayer.Repositories;
using Betto.Helpers.Extensions;
using Betto.Model.Models;
using Betto.Model.ViewModels;
using Betto.Resources.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Betto.Services.GameService
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILeagueRepository _leagueRepository;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public GameService(IGameRepository gameRepository,
            ILeagueRepository leagueRepository,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _gameRepository = gameRepository;
            _leagueRepository = leagueRepository;
            _localizer = localizer;
        }

        public async Task<RequestResponseModel<ICollection<GameViewModel>>> GetLeagueGamesAsync(int leagueId)
        {
            var doesLeagueExist = await CheckDoesLeagueExistAsync(leagueId);

            if (!doesLeagueExist)
            {
                return new RequestResponseModel<ICollection<GameViewModel>>(StatusCodes.Status404NotFound,
                    new List<ErrorViewModel>
                    {
                        ErrorViewModel.Factory.NewErrorFromMessage(_localizer["LeagueNotFoundErrorMessage"]
                            .Value)
                    },
                    null);
            }

            var leagueGames = (await _gameRepository.GetLeagueGamesAsync(leagueId))
                .Select(g => (GameViewModel) g)
                .ToList()
                .GetEmptyIfNull();

            return new RequestResponseModel<ICollection<GameViewModel>>(StatusCodes.Status200OK, 
                Enumerable.Empty<ErrorViewModel>(), 
                leagueGames);
        }

        public async Task<bool> CheckDoesLeagueExistAsync(int leagueId) =>
            await _leagueRepository.GetLeagueByIdAsync(leagueId, false, false) != null;
    }
}