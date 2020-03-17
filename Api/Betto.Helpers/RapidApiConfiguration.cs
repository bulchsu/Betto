namespace Betto.Helpers
{
    public class RapidApiConfiguration
    {
        public string RapidApiUrl { get; set; }
        public string RapidApiHost { get; set; }
        public string HostHeaderName { get; set; }
        public string RapidApiKey { get; set; }
        public string KeyHeaderName { get; set; }
        public string JsonBackupDirectory { get; set; }
        public string LeaguesRoute { get; set; }
        public string TeamsRoute { get; set; }
        public int LeaguesAmount { get; set; }
        public string BackupFileSuffix { get; set; }
    }
}
