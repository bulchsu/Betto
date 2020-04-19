using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.Configuration;
using Betto.Model.Entities;
using Betto.RapidApiCommunication.Parsers;
using Microsoft.Extensions.Options;

namespace Betto.RapidApiCommunication.Managers
{
    public class TeamManager : BaseApiManager, ITeamManager
    {
        private readonly IParser<TeamEntity> _teamParser;

        public TeamManager(IOptions<RapidApiConfiguration> configuration, 
            IParser<TeamEntity> teamParser, 
            ApiClient apiClient)
            : base(configuration, apiClient)
        {
            _teamParser = teamParser;
        }

        public async Task<IEnumerable<TeamEntity>> GetTeamsAsync(IEnumerable<int> leagueIds)
        {
            var tasks = leagueIds.Select(GetLeagueTeamsAsync).ToList();
            var teams = (await Task.WhenAll(tasks)).SelectMany(t => t);

            return teams;
        }

        private async Task<IEnumerable<TeamEntity>> GetLeagueTeamsAsync(int leagueId)
        {
            var url = GetLeagueTeamsUrl(leagueId);
            var headers = GetRapidApiAuthenticationHeaders();
            
            var rawJson = await ApiClient.GetAsync(url, string.Empty, headers);

            var teams = _teamParser.Parse(rawJson);
            ConnectTeamsToCorrectLeague(leagueId, teams);

            return teams;
        }

        private string GetLeagueTeamsUrl(int leagueId)
            => string.Concat(Configuration.TeamsUrl, leagueId);

        private static void ConnectTeamsToCorrectLeague(int leagueId, IEnumerable<TeamEntity> teams)
        {
            foreach (var team in teams)
            {
                team.LeagueId = leagueId;
            }
        }
    }
}
