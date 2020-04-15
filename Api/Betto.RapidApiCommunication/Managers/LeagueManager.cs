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

        public LeagueManager(IOptions<RapidApiConfiguration> configuration, 
            ILogger logger, 
            IParser<LeagueEntity> leagueParser, 
            ApiClient apiClient)
            : base(configuration, logger, apiClient)
        {
            _leagueParser = leagueParser;
        }

        public async Task<IEnumerable<LeagueEntity>> GetLeaguesAsync(ICollection<int> leaguesToImportIds)
        {
            var url = GetLeaguesUrl();
            var headers = GetRapidApiAuthenticationHeaders();

            var rawJson = await ApiClient.GetAsync(url, string.Empty, headers);

            Logger.LogToFile("leagues", rawJson);

            var leagues = _leagueParser.Parse(rawJson);

            return leagues.Where(l => leaguesToImportIds.Contains(l.RapidApiExternalId));
        }

        private string GetLeaguesUrl()
            => string.Concat(Configuration.RapidApiUrl, Configuration.LeaguesRoute);
    }
}
