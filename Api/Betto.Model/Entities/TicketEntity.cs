using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Betto.Model.Constants;
using Betto.Model.Models;
using Betto.Model.WriteModels;

namespace Betto.Model.Entities
{
    public class TicketEntity
    {
        [Key]
        public int TicketId { get; set; }
        public UserEntity User { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public ICollection<EventEntity> Events { get; set; }
        public DateTime CreationDateTime { get; set; }
        [Range(TicketConstants.MinimumStake, TicketConstants.MaximumStake)]
        public double Stake { get; set; }
        public float TotalConfirmedRate { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime? RevealDateTime { get; set; }

        public static explicit operator TicketEntity(TicketWriteModel ticket) => ticket == null
            ? null
            : new TicketEntity
            {
                UserId = ticket.UserId,
                Events = ticket.Events.Select(e => (EventEntity) e).ToList(),
                CreationDateTime = DateTime.Now,
                Stake = ticket.Stake,
                Status = StatusEnum.Pending
            };
    }
}
