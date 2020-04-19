using Betto.Model.Models;

namespace Betto.Model.WriteModels
{
    public class TicketEventWriteModel
    {
        public int GameId { get; set; }
        public BetTypeEnum BetType { get; set; }
    }
}
