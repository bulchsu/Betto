using Betto.Model.Entities;
using Betto.Model.Models;

namespace Betto.Model.ViewModels
{
    public class TicketEventViewModel
    {
        public int EventId { get; set; }
        public int TicketId { get; set; }
        public GameViewModel Game { get; set; }
        public BetTypeEnum BetType { get; set; }
        public float ConfirmedRate { get; set; }
        public StatusEnum EventStatus { get; set; }

        public static explicit operator TicketEventViewModel(EventEntity eventEntity) => eventEntity == null
            ? null
            : new TicketEventViewModel
            {
                EventId = eventEntity.EventId,
                TicketId = eventEntity.TicketId,
                Game = (GameViewModel) eventEntity.Game,
                BetType = eventEntity.BetType,
                ConfirmedRate = eventEntity.ConfirmedRate,
                EventStatus = eventEntity.EventStatus
            };
    }
}
