using System.Collections.Generic;

namespace Betto.Model.Models
{
    public partial class LeagueTableModel
    {
        private LeagueTableModel(int leagueId, string leagueName, Queue<TeamStatisticsModel> table)
        {
            LeagueId = leagueId;
            LeagueName = leagueName;
            Table = table;
        }

        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public Queue<TeamStatisticsModel> Table { get; set; }
    }
}
