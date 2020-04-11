using System.Collections.Generic;

namespace Betto.Model.DTO
{
    public partial class LeagueTableDTO
    {
        private LeagueTableDTO(int leagueId, string leagueName, Queue<TeamStatisticsDTO> table)
        {
            LeagueId = leagueId;
            LeagueName = leagueName;
            Table = table;
        }

        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public Queue<TeamStatisticsDTO> Table { get; set; }
    }
}
