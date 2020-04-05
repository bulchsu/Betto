using Betto.Model.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Betto.Configuration;

namespace Betto.Helpers
{
    public sealed class TeamParser : BaseParser, ITeamParser
    {
        public TeamParser(IOptions<RapidApiConfiguration> apiConfiguration, ILogger logger)
            : base(apiConfiguration, logger)
        {

        }

        public async Task<IEnumerable<IEnumerable<TeamEntity>>> GetTeamsAsync()
        {
            var tasks = new List<Task<IEnumerable<TeamEntity>>>();
            
            for (int i = 1; i < _apiConfiguration.LeaguesAmount + 1; i++)
                tasks.Add(GetLeagueTeamsAsync(i));

            var teams = await Task.WhenAll(tasks);

            return teams;
        }

        private async Task<IEnumerable<TeamEntity>> GetLeagueTeamsAsync(int leagueId)
        {
            var url = GetTeamsUrl(leagueId);

            var jsonString = await ExecuteUrlAsync(url, Method.GET);
            _logger.LogToFile($"league_{leagueId}_teams", jsonString);

            var teams = ParseTeams(jsonString);

            return teams;
        }

        private IEnumerable<TeamEntity> ParseTeams(string rawJson)
        {
            var teams = JsonConvert.DeserializeAnonymousType(rawJson, new
            {
                Api = new
                {
                    Teams = default(List<TeamEntity>)
                }
            })?.Api?.Teams;

            var venues = JsonConvert.DeserializeAnonymousType(rawJson, new
            {
                Api = new
                {
                    Teams = default(List<VenueEntity>) //it has to be named Teams, if not the parse will fail this way
                }
            })?.Api?.Teams;

            for (int i = 0; i < teams.Count; i++)
                teams.ElementAt(i).Venue = venues.ElementAt(i);

            return teams;
        }

        private string GetTeamsUrl(int leagueId)
            => string.Concat(_apiConfiguration.RapidApiUrl, _apiConfiguration.TeamsRoute, leagueId);
    }
}
