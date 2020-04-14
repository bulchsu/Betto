using System;
using System.Collections.Generic;
using System.Linq;
using Betto.Model.Entities;
using Betto.Model.Models;

namespace Betto.Model.DTO
{
    public class TicketDTO
    {
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public ICollection<TicketEventDTO> Events { get; set; }
        public DateTime CreationDateTime { get; set; }
        public double Stake { get; set; }
        public float TotalConfirmedRate { get; set; }
        public ResultEnum Status { get; set; }
        public DateTime? RevealDateTime { get; set; }

        public static explicit operator TicketDTO(TicketEntity ticket) => ticket == null
            ? null
            : new TicketDTO
            {
                TicketId = ticket.TicketId,
                UserId = ticket.UserId,
                Events = ticket.Events.Select(t => (TicketEventDTO)t).ToList(),
                CreationDateTime = ticket.CreationDateTime,
                Stake = ticket.Stake,
                TotalConfirmedRate = ticket.TotalConfirmedRate,
                Status = ticket.Status,
                RevealDateTime = ticket.RevealDateTime
            };
    }
}
