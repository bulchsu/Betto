using Betto.Configuration;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Betto.Helpers
{
    public abstract class BaseParser
    {
        protected readonly RapidApiConfiguration _apiConfiguration;
        protected readonly ILogger _logger;

        protected BaseParser(IOptions<RapidApiConfiguration> apiConfiguration, ILogger logger)
        {
            this._apiConfiguration = apiConfiguration.Value;
            this._logger = logger;
        }

        protected async Task<string> ExecuteUrlAsync(string url, Method httpMethod)
        {
            var client = new RestClient(url);
            var request = new RestRequest(httpMethod);

            request.AddHeader(_apiConfiguration.HostHeaderName, _apiConfiguration.RapidApiHost);
            request.AddHeader(_apiConfiguration.KeyHeaderName, _apiConfiguration.RapidApiKey);

            var response = await client.ExecuteAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"Could not retrieve data from {url}");

            return response.Content;
        }
    }
}
