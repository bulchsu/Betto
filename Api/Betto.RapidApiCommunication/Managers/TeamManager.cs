using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Configuration;
using Betto.Helpers;
using Betto.Model.Entities;
using Betto.RapidApiCommunication.Parsers;
using Microsoft.Extensions.Options;

namespace Betto.RapidApiCommunication.Managers
{
    public class TeamManager : BaseApiManager, ITeamManager
    {
        private readonly IParser<TeamEntity> _teamParser;

        public TeamManager(IOptions<RapidApiConfiguration> configuration, ILogger logger, IParser<TeamEntity> teamParser, ApiClient apiClient)
            : base(configuration, logger, apiClient)
        {
            _teamParser = teamParser;
        }

        public async Task<IEnumerable<IEnumerable<TeamEntity>>> GetTeamsAsync()
        {
            var tasks = new List<Task<IEnumerable<TeamEntity>>>();

            for (int i = 1; i < Configuration.LeaguesAmount + 1; i++)
            {
                tasks.Add(GetLeagueTeamsAsync(i));
            }

            var teams = await Task.WhenAll(tasks);

            return teams;
        }

        private async Task<IEnumerable<TeamEntity>> GetLeagueTeamsAsync(int leagueId)
        {
            var url = GetTeamsUrl(leagueId);
            var headers = GetRapidApiAuthenticationHeaders();
            
            var rawJson = await ApiClient.GetAsync(url, string.Empty, headers);
            
            Logger.LogToFile($"league_{leagueId}_teams", rawJson);

            var teams = _teamParser.Parse(rawJson);

            return teams;
        }

        private string GetTeamsUrl(int leagueId)
            => string.Concat(Configuration.RapidApiUrl, Configuration.TeamsRoute, leagueId);
    }
}
