using System.Collections.Generic;

namespace Betto.Model.Models
{
    public partial class LeagueTable
    {
        private LeagueTable(int leagueId, string leagueName, Queue<TeamStatistics> table)
        {
            LeagueId = leagueId;
            LeagueName = leagueName;
            Table = table;
        }

        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public Queue<TeamStatistics> Table { get; set; }
    }
}
