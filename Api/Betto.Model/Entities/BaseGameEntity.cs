using System.ComponentModel.DataAnnotations;

namespace Betto.Model.Entities
{
    /// <summary>
    /// Base for the GameEntity and ScoreEntity. Solution taken from https://stackoverflow.com/questions/41261738/ef-core-using-id-as-primary-key-and-foreign-key-at-same-time
    /// </summary>
    public abstract class BaseGameEntity
    {
        [Key]
        public int GameId { get; set; }
    }
}
