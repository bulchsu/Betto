using Betto.Model.Entities;
using System;

namespace Betto.Model.DTO
{
    public class LeagueDTO
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

        public static explicit operator LeagueDTO(LeagueEntity league)
            => league == null ? null : new LeagueDTO
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
                Standings = league.Standings
            };
    }
}
