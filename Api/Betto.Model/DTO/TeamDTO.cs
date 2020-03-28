using Betto.Model.Entities;

namespace Betto.Model.DTO
{
    public class TeamDTO
    {
        public int TeamId { get; set; }
        public int LeagueId { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Country { get; set; }
        public string IsNational { get; set; }
        public VenueDTO Venue { get; set; }

        public static explicit operator TeamDTO(TeamEntity team)
            => team == null ? null : new TeamDTO
            {
                TeamId = team.TeamId,
                LeagueId = team.LeagueId,
                Name = team.Name,
                Logo = team.Logo,
                Country = team.Country,
                IsNational = team.IsNational,
                Venue = (VenueDTO)team.Venue
            };
    }
}
