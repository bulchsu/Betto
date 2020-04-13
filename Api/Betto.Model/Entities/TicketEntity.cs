using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Betto.Model.Constants;
using Betto.Model.Models;

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
        public TicketStatus Status { get; set; }
        public DateTime? PendingDateTime { get; set; }
    }
}
