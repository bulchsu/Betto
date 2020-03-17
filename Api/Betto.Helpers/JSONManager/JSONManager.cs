using Betto.Model.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using System.Linq;

namespace Betto.Helpers.JSONManager
{
    public class JSONManager
    {
        private readonly RapidApiConfiguration _apiConfiguration;

        public JSONManager(RapidApiConfiguration configuration)
        {
            this._apiConfiguration = configuration;
        }

        public async Task<IEnumerable<LeagueEntity>> GetLeaguesAsync()
        {
            var url = GetLeaguesUrl();
            var filePath = GetBackupFilePath();

            var jsonString = await ExecuteUrlAsync(filePath, url);
            Backup(filePath, jsonString);

            var results = ParseLeagues(jsonString);
            var leagues = results.Take(_apiConfiguration.LeaguesAmount);
            var teams = await GetTeamsAsync();

            for (int i = 0; i < leagues.Count(); i++)
                leagues.ElementAt(i).Teams = teams.ElementAt(i).ToList();

            return leagues;
        }

        private async Task<IEnumerable<IEnumerable<TeamEntity>>> GetTeamsAsync()
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
            var filePath = GetBackupFilePath();

            var jsonString = await ExecuteUrlAsync(filePath, url);
            Backup(filePath, jsonString);

            var teams = ParseTeams(jsonString);

            return teams;
        }

        private async Task<string> ExecuteUrlAsync(string filePath, string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.AddHeader(_apiConfiguration.HostHeaderName, _apiConfiguration.RapidApiHost);
            request.AddHeader(_apiConfiguration.KeyHeaderName, _apiConfiguration.RapidApiKey);

            var response = await client.ExecuteAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Could not retrieve data from {url}");

            return response.Content;
        }

        private void Backup(string filePath, string rawJson) 
            => File.WriteAllText(filePath, rawJson);

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

        private string GetLeaguesUrl()
            => string.Concat(_apiConfiguration.RapidApiUrl, _apiConfiguration.LeaguesRoute);

        private string GetTeamsUrl(int leagueId)
            => string.Concat(_apiConfiguration.RapidApiUrl, _apiConfiguration.TeamsRoute, leagueId);

        private string GetBackupFilePath()
            => string.Concat(_apiConfiguration.JsonBackupDirectory, DateTime.Now.ToString("yyyyMMdd_HHmmssfff"), _apiConfiguration.BackupFileSuffix);
    }
}
