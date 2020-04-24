using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Betto.Model.Entities
{
    public class FixturesEntity
    {
        [Key]
        public int FixturesId { get; set; }
        [Required]
        public bool Events { get; set; }
        [Required]
        public bool Lineups { get; set; }
        [Required]
        public bool Statistics { get; set; }
        [Required]
        public bool PlayerStatistics { get; set; }
        [Required]
        public CoverageEntity Coverage { get; set; }
        [ForeignKey("Coverage")]
        public int CoverageId { get; set; }
    }
}
