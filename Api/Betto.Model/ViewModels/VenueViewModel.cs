using Betto.Model.Entities;

namespace Betto.Model.ViewModels
{
    public class VenueViewModel
    {
        public string Name { get; set; }
        public string Surface { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Capacity { get; set; }

        public static explicit operator VenueViewModel(VenueEntity venue)
            => venue == null ? null : new VenueViewModel
            {
                Name = venue.Name,
                Surface = venue.Surface,
                Address = venue.Address,
                City = venue.City,
                Capacity = venue.Capacity
            };
    }
}
