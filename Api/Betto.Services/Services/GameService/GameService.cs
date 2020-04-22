using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.DataAccessLayer.Repositories;
using Betto.Helpers.Extensions;
using Betto.Model.Models;
using Betto.Model.ViewModels;
using Betto.Resources.Shared;
using Betto.Services.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Betto.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILeagueValidator _leagueValidator;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public GameService(IGameRepository gameRepository,
            ILeagueValidator leagueValidator,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _gameRepository = gameRepository;
            _leagueValidator = leagueValidator;
            _localizer = localizer;
        }

        public async Task<RequestResponseModel<ICollection<GameViewModel>>> GetLeagueGamesAsync(int leagueId)
        {
            var doesLeagueExist = await _leagueValidator.CheckDoesTheLeagueExistAsync(leagueId);

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

        public async Task<RequestResponseModel<GameViewModel>> GetGameByIdAsync(int gameId, bool includeRates)
        {
            var game = (GameViewModel) await _gameRepository.GetGameByIdAsync(gameId, includeRates);

            if (game == null)
            {
                return new RequestResponseModel<GameViewModel>(StatusCodes.Status404NotFound,
                    new List<ErrorViewModel>
                    {
                        ErrorViewModel.Factory.NewErrorFromMessage(_localizer["GameNotFoundErrorMessage"]
                            .Value)
                    },
                    null);
            }

            return new RequestResponseModel<GameViewModel>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(),
                game);
        }
    }
}