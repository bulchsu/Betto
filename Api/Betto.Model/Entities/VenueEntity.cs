using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Betto.Model.Entities
{
    public class VenueEntity
    {
        [Key]
        public int VenueId { get; set; }
        [MaxLength(200)]
        [JsonProperty("venue_name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [MaxLength(100)]
        [JsonProperty("venue_surface", NullValueHandling = NullValueHandling.Ignore)]
        public string Surface { get; set; }
        [MaxLength(200)]
        [JsonProperty("venue_address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }
        [MaxLength(100)]
        [JsonProperty("venue_city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        [JsonProperty("venue_capacity", NullValueHandling = NullValueHandling.Ignore)]
        public int Capacity { get; set; }
        [Required]
        public TeamEntity Team { get; set; }
        [ForeignKey("Team")]
        public int TeamId { get; set; }
    }
}
