using Betto.Model.Entities;
using Betto.Model.Models;

namespace Betto.Model.DTO
{
    public class TicketEventDTO
    {
        public int EventId { get; set; }
        public int TicketId { get; set; }
        public int GameId { get; set; }
        public EventType Type { get; set; }
        public float ConfirmedRate { get; set; }

        public static explicit operator TicketEventDTO(EventEntity eventEntity) => eventEntity == null
            ? null
            : new TicketEventDTO
            {
                EventId = eventEntity.EventId,
                TicketId = eventEntity.TicketId,
                GameId = eventEntity.GameId,
                Type = eventEntity.Type,
                ConfirmedRate = eventEntity.ConfirmedRate
            };
    }
}
