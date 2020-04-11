namespace Betto.Model.DTO
{
    public class TeamStatisticsDTO
    {
        public int Position { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int Points { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsLost { get; set; }
        public int WonGamesAmount { get; set; }
        public int TiedMatchesAmount { get; set; }
        public int LostGamesAmount { get; set; }
    }
}
