using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Models;
using Betto.Model.ViewModels;

namespace Betto.Services.GameService
{
    public interface IGameService
    {
        Task<RequestResponseModel<ICollection<GameViewModel>>> GetLeagueGamesAsync(int leagueId);
        Task<RequestResponseModel<GameViewModel>> GetGameByIdAsync(int gameId, bool includeRates);
    }
}