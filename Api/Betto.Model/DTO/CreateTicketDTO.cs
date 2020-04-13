using System.Collections.Generic;

namespace Betto.Model.DTO
{
    public class CreateTicketDTO
    {
        public int UserId { get; set; }
        public float Stake { get; set; }
        public ICollection<CreateTicketEventDTO> Events { get; set; }
    }
}
