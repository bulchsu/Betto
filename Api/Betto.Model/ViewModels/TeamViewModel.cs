using Betto.Model.Entities;

namespace Betto.Model.ViewModels
{
    public class TeamViewModel
    {
        public int TeamId { get; set; }
        public int LeagueId { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Country { get; set; }
        public string IsNational { get; set; }
        public VenueViewModel Venue { get; set; }

        public static explicit operator TeamViewModel(TeamEntity team)
            => team == null ? null : new TeamViewModel
            {
                TeamId = team.TeamId,
                LeagueId = team.LeagueId,
                Name = team.Name,
                Logo = team.Logo,
                Country = team.Country,
                IsNational = team.IsNational,
                Venue = (VenueViewModel)team.Venue
            };
    }
}
