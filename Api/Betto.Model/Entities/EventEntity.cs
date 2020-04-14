using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Betto.Model.Models;
using Betto.Model.WriteModels;

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

        public static explicit operator EventEntity(TicketEventWriteModel eventViewModel) => eventViewModel == null
            ? null
            : new EventEntity
            {
                GameId = eventViewModel.GameId,
                Type = eventViewModel.Type
            };
    }
}
