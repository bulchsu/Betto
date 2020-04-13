using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public EventType Type { get; set; }
        public float ConfirmedRate { get; set; }
    }
}
