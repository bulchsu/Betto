namespace Betto.Model.ViewModels
{
    public class UserRankingPositionViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public int RankingPosition { get; set; }
        public double HighestPrice { get; set; }
        public double TotalWonAmount { get; set; }
    }
}
