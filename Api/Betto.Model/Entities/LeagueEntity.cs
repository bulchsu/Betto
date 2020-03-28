using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Betto.Model.Entities
{
    public class LeagueEntity
    {
        [Key]
        public int LeagueId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Type { get; set; }
        [MaxLength(100)]
        public string Country { get; set; }
        [MaxLength(10)]
        [Column(TypeName = "varchar(10)")]
        public string CountryCode { get; set; }
        [Required]
        public int Season { get; set; }
        [Required]
        [JsonProperty("season_start", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime SeasonStart { get; set; }
        [Required]
        [JsonProperty("season_end", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime SeasonEnd { get; set; }
        [MaxLength(300)]
        public string Logo { get; set; }
        [MaxLength(300)]
        public string Flag { get; set; }
        [Required]
        public bool Standings { get; set; }
        [Required]
        [JsonProperty("is_current", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsCurrent { get; set; }
        [Required]
        public CoverageEntity Coverage { get; set; }
        [Required]
        public ICollection<TeamEntity> Teams { get; set; }
    }
}
