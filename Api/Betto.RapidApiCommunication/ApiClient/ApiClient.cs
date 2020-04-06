using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using RestSharp;

namespace Betto.RapidApiCommunication
{
    public class ApiClient
    {
        public async Task<string> GetAsync(string resourceUrl, string body, ICollection<KeyValuePair<string, string>> headers)
        {
            var client = new RestClient(resourceUrl);
            var request = new RestRequest(Method.GET);

            request.AddHeaders(headers)
                .AddJsonBody(body);

            var response = await client.ExecuteAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Could not retrieve data from {resourceUrl}");
            }

            return response.Content;
        }
    }
}
