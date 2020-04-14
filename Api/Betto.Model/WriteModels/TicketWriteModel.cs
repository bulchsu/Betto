using System.Collections.Generic;

namespace Betto.Model.WriteModels
{
    public class TicketWriteModel
    {
        public int UserId { get; set; }
        public float Stake { get; set; }
        public ICollection<TicketEventWriteModel> Events { get; set; }
    }
}
