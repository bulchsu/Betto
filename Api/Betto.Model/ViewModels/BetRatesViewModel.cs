using Betto.Model.Entities;

namespace Betto.Model.ViewModels
{
    public class BetRatesViewModel
    {
        public int BetRateId { get; set; }
        public int GameId { get; set; }
        public float HomeTeamWinRate { get; set; }
        public float TieRate { get; set; }
        public float AwayTeamWinRate { get; set; }

        public static explicit operator BetRatesViewModel(BetRatesEntity rates) => rates == null
            ? null
            : new BetRatesViewModel
            {
                BetRateId = rates.BetRateId,
                GameId = rates.GameId,
                HomeTeamWinRate = rates.HomeTeamWinRate,
                TieRate = rates.TieRate,
                AwayTeamWinRate = rates.AwayTeamWinRate
            };
    }
}
