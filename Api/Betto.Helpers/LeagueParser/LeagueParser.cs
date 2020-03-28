using Betto.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using System.Linq;
using Microsoft.Extensions.Options;
using Betto.Helpers.Configuration;

namespace Betto.Helpers
{
    public sealed class LeagueParser : BaseParser, ILeagueParser
    {
        public LeagueParser(IOptions<RapidApiConfiguration> configuration, ILogger logger)
            : base(configuration, logger)
        {
          
        }

        public async Task<IEnumerable<LeagueEntity>> GetLeaguesAsync()
        {
            var url = GetLeaguesUrl();

            var jsonString = await ExecuteUrlAsync(url, Method.GET);
            _logger.LogToFile("leagues", jsonString);

            var results = ParseLeagues(jsonString);
            var leagues = results.Take(_apiConfiguration.LeaguesAmount);

            return leagues;
        }

        private IEnumerable<LeagueEntity> ParseLeagues(string rawJson)
        {
            var leagues = JsonConvert.DeserializeAnonymousType(rawJson, new 
            { 
                Api = new 
                { 
                    Leagues = default(List<LeagueEntity>) 
                } 
            })?.Api?.Leagues;

            return leagues;
        }

        private string GetLeaguesUrl()
            => string.Concat(_apiConfiguration.RapidApiUrl, _apiConfiguration.LeaguesRoute);
    }
}
