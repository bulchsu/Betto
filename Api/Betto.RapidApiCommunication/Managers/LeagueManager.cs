using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.Configuration;
using Betto.Model.Entities;
using Betto.RapidApiCommunication.Parsers;
using Microsoft.Extensions.Options;

namespace Betto.RapidApiCommunication.Managers
{
    public class LeagueManager : BaseApiManager, ILeagueManager
    {
        private readonly IParser<LeagueEntity> _leagueParser;

        public LeagueManager(IOptions<RapidApiConfiguration> configuration,  
            IParser<LeagueEntity> leagueParser, 
            ApiClient apiClient)
            : base(configuration, apiClient)
        {
            _leagueParser = leagueParser;
        }

        public async Task<IEnumerable<LeagueEntity>> GetLeaguesAsync(ICollection<int> leaguesToImportIds)
        {
            var url = Configuration.LeaguesUrl;
            var headers = GetRapidApiAuthenticationHeaders();

            var rawJson = await ApiClient.GetAsync(url, string.Empty, headers);

            var leagues = _leagueParser.Parse(rawJson);

            return leagues.Where(l => leaguesToImportIds.Contains(l.RapidApiExternalId));
        }
    }
}
