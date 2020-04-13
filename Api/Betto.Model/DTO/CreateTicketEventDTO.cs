using Betto.Model.Models;

namespace Betto.Model.DTO
{
    public class CreateTicketEventDTO
    {
        public int GameId { get; set; }
        public EventType Type { get; set; }
    }
}
