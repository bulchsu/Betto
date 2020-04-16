using Betto.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Betto.Model.ViewModels
{
    public class LeagueViewModel
    {
        public int LeagueId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Country { get; set; }
        public int Season { get; set; }
        public DateTime SeasonStart { get; set; }
        public DateTime SeasonEnd { get; set; }
        public string Logo { get; set; }
        public string Flag { get; set; }
        public bool Standings { get; set; }
        public ICollection<TeamViewModel> Teams { get; set; }
        public ICollection<GameViewModel> Games { get; set; }

        public static explicit operator LeagueViewModel(LeagueEntity league)
            => league == null ? null : new LeagueViewModel
            {
                LeagueId = league.LeagueId,
                Name = league.Name,
                Type = league.Type,
                Country = league.Country,
                Season = league.Season,
                SeasonStart = league.SeasonStart,
                SeasonEnd = league.SeasonEnd,
                Logo = league.Logo,
                Flag = league.Flag,
                Standings = league.Standings,
                Teams = league.Teams
                    ?.Select(t =>  (TeamViewModel) t)
                    .ToList(),
                Games = league.Games
                    ?.Select(g => (GameViewModel) g)
                    .ToList()
            };
    }
}
