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
    public class LeagueManager : BaseApiManager, ILeagueManager
    {
        private readonly IParser<LeagueEntity> _leagueParser;

        public LeagueManager(IOptions<RapidApiConfiguration> configuration, ILogger logger, IParser<LeagueEntity> leagueParser)
            : base(configuration, logger)
        {
            _leagueParser = leagueParser;
        }

        public async Task<IEnumerable<LeagueEntity>> GetLeaguesAsync()
        {
            var url = GetLeaguesUrl();
            var headers = GetRapidApiAuthenticationHeaders();

            var rawJson = await ApiClient.GetAsync(url, string.Empty, headers);

            Logger.LogToFile("leagues", rawJson);

            var leagues = _leagueParser.Parse(rawJson);

            return leagues.Take(Configuration.LeaguesAmount);
        }

        private string GetLeaguesUrl()
            => string.Concat(Configuration.RapidApiUrl, Configuration.LeaguesRoute);
    }
}
