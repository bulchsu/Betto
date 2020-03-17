using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Betto.Model.Entities
{
    public class VenueEntity
    {
        [Key]
        public int VenueId { get; set; }
        [Required]
        [MaxLength(200)]
        [JsonProperty("venue_name", NullValueHandling = NullValueHandling.Ignore)]
        public string VenueName { get; set; }
        [Required]
        [MaxLength(100)]
        [JsonProperty("venue_surface", NullValueHandling = NullValueHandling.Ignore)]
        public string VenueSurface { get; set; }
        [MaxLength(200)]
        [JsonProperty("venue_address", NullValueHandling = NullValueHandling.Ignore)]
        public string VenueAddress { get; set; }
        [Required]
        [MaxLength(100)]
        [JsonProperty("venue_city", NullValueHandling = NullValueHandling.Ignore)]
        public string VenueCity { get; set; }
        [Required]
        [JsonProperty("venue_capacity", NullValueHandling = NullValueHandling.Ignore)]
        public int VenueCapacity { get; set; }
        [Required]
        public TeamEntity Team { get; set; }
        [ForeignKey("Team")]
        public int TeamId { get; set; }
    }
}
