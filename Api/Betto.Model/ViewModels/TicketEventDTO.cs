using Betto.Model.Entities;
using Betto.Model.Models;

namespace Betto.Model.ViewModels
{
    public class TicketEventViewModel
    {
        public int EventId { get; set; }
        public int TicketId { get; set; }
        public int GameId { get; set; }
        public EventType Type { get; set; }
        public float ConfirmedRate { get; set; }
        public ResultEnum Result { get; set; }

        public static explicit operator TicketEventViewModel(EventEntity eventEntity) => eventEntity == null
            ? null
            : new TicketEventViewModel
            {
                EventId = eventEntity.EventId,
                TicketId = eventEntity.TicketId,
                GameId = eventEntity.GameId,
                Type = eventEntity.Type,
                ConfirmedRate = eventEntity.ConfirmedRate,
                Result = eventEntity.HiddenEventResult
            };
    }
}
