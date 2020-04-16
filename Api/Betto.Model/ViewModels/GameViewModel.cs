using System;
using Betto.Model.Entities;

namespace Betto.Model.ViewModels
{
    public class GameViewModel
    {
        public int GameId { get; set; }
        public int LeagueId { get; set; }
        public DateTime EventDate { get; set; }
        public string Round { get; set; }
        public string Venue { get; set; }
        public string Referee { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public BetRatesViewModel Rates { get; set; }

        public static explicit operator GameViewModel(GameEntity game) => game == null
            ? null
            : new GameViewModel
            {
                GameId = game.GameId,
                LeagueId = game.LeagueId,
                EventDate = game.EventDate,
                Round = game.Round,
                Venue = game.Venue,
                Referee = game.Referee,
                HomeTeamId = game.HomeTeamId,
                AwayTeamId = game.AwayTeamId,
                Rates = (BetRatesViewModel) game.Rates
            };
    }
}
