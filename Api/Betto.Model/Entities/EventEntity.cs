using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Betto.Model.DTO;
using Betto.Model.Models;

namespace Betto.Model.Entities
{
    public class EventEntity
    {
        [Key]
        public int EventId { get; set; }
        public TicketEntity Ticket { get; set; }
        [ForeignKey(nameof(Ticket))]
        public int TicketId { get; set; }
        public GameEntity Game { get; set; }
        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }
        public EventType Type { get; set; }
        public float ConfirmedRate { get; set; }
        public ResultEnum HiddenEventResult { get; set; }

        public static explicit operator EventEntity(CreateTicketEventDTO eventDto) => eventDto == null
            ? null
            : new EventEntity
            {
                GameId = eventDto.GameId,
                Type = eventDto.Type
            };
    }
}
