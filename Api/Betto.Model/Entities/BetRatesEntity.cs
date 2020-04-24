using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Betto.Model.Entities
{
    public class BetRatesEntity
    {
        [Key]
        public int BetRateId { get; set; }
        public GameEntity Game { get; set; }
        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }
        public float HomeTeamWinRate { get; set; }
        public float TieRate { get; set; }
        public float AwayTeamWinRate { get; set; }
    }
}
