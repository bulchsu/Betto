namespace Betto.Configuration
{
    public class RapidApiConfiguration
    {
        public string RapidApiHost { get; set; }
        public string HostHeaderName { get; set; }
        public string RapidApiKey { get; set; }
        public string KeyHeaderName { get; set; }
        public string LeaguesUrl { get; set; }
        public string TeamsUrl { get; set; }
        public string FixturesUrl { get; set; }
        public int InitialLeaguesAmount { get; set; }
        public string Timezone { get; set; }
    }
}
