using Betto.Model.Entities;

namespace Betto.Model.DTO
{
    public class VenueDTO
    {
        public string Name { get; set; }
        public string Surface { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int Capacity { get; set; }

        public static explicit operator VenueDTO(VenueEntity venue)
            => new VenueDTO
            {
                Name = venue.Name,
                Surface = venue.Surface,
                Address = venue.Address,
                City = venue.City,
                Capacity = venue.Capacity
            };
    }
}
