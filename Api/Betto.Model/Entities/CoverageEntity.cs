using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Betto.Model.Entities
{ 
    public class CoverageEntity
    {
        [Key]
        public int CoverageId { get; set; }
        [Required]
        public bool Standings { get; set; }
        [Required]
        public FixturesEntity Fixtures { get; set; }
        [Required]
        public bool Players { get; set; }
        [Required]
        public bool TopScorers { get; set; }
        [Required]
        public bool Predictions { get; set; }
        [Required]
        public bool Odds { get; set; }
        [Required]
        public LeagueEntity League { get; set; }
        [ForeignKey("League")]
        public int LeagueId { get; set; }
    }
}
