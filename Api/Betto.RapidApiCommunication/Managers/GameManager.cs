using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.Configuration;
using Betto.Helpers;
using Betto.Model.Entities;
using Betto.RapidApiCommunication.Parsers;
using Microsoft.Extensions.Options;

namespace Betto.RapidApiCommunication.Managers
{
    public class GameManager : BaseApiManager, IGameManager
    {
        private readonly IParser<GameEntity> _gameParser;

        public GameManager(IParser<GameEntity> gameParser,
            IOptions<RapidApiConfiguration> configuration,
            ILogger logger,
            ApiClient apiClient)
            : base(configuration, logger, apiClient)
        {
            _gameParser = gameParser;
        }

        public async Task<IEnumerable<GameEntity>> GetGamesAsync(IEnumerable<int> leagueIds)
        {
            var tasks = leagueIds.Select(GetLeagueMatchesAsync).ToList();
            var games = (await Task.WhenAll(tasks)).SelectMany(game => game);

            return games;
        }

        private async Task<IEnumerable<GameEntity>> GetLeagueMatchesAsync(int leagueId)
        {
            var url = GetLeagueMatchesUrl(leagueId);
            var headers = GetRapidApiAuthenticationHeaders();

            var jsonResponse = await ApiClient.GetAsync(url, string.Empty, headers);

            Logger.LogToFile($"league_{leagueId}_games", jsonResponse);

            var leagueGames = _gameParser.Parse(jsonResponse);

            return leagueGames;
        }

        private string GetLeagueMatchesUrl(int leagueId) =>
            string.Concat(Configuration.RapidApiUrl, Configuration.LeagueFixturesRoute, leagueId, 
                "?timezone=", Configuration.Timezone);
    }
}
