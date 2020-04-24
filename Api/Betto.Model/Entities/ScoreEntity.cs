using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Betto.Model.Entities
{
    public class ScoreEntity : BaseGameEntity
    {
        [MaxLength(20)]
        public string HalfTime { get; set; }
        [MaxLength(20)]
        public string FullTime { get; set; }
        public string ExtraTime { get; set; }
        public string Penalty { get; set; }
        [ForeignKey(nameof(GameId))]
        public GameEntity Game { get; set; }
    }
}
