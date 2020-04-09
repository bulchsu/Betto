using System.Collections.Generic;
using Betto.Configuration;
using Betto.Helpers;
using Microsoft.Extensions.Options;

namespace Betto.RapidApiCommunication.Managers
{
    public abstract class BaseApiManager
    {
        protected BaseApiManager(IOptions<RapidApiConfiguration> configuration, ILogger logger, ApiClient apiClient)
        {
            Logger = logger;
            Configuration = configuration.Value;
            ApiClient = apiClient;
        }

        protected RapidApiConfiguration Configuration { get; }
        protected ILogger Logger { get; }
        protected ApiClient ApiClient { get; }

        protected ICollection<KeyValuePair<string, string>> GetRapidApiAuthenticationHeaders()
        {
            return new Dictionary<string, string>
            {
                { Configuration.HostHeaderName, Configuration.RapidApiHost },
                { Configuration.KeyHeaderName, Configuration.RapidApiKey }
            };
        }
    }
}
