using System.Collections.Generic;
using Betto.Configuration;
using Betto.Helpers;
using Microsoft.Extensions.Options;

namespace Betto.RapidApiCommunication.Managers
{
    public abstract class BaseApiManager
    {
        protected BaseApiManager(IOptions<RapidApiConfiguration> configuration, ILogger logger)
        {
            Logger = logger;
            Configuration = configuration.Value;
        }

        protected RapidApiConfiguration Configuration { get; }
        protected ILogger Logger { get; }
        protected ApiClient ApiClient { get; } = new ApiClient();

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
