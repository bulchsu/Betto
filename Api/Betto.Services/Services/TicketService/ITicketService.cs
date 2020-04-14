using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.DTO;
using Betto.Model.Models;

namespace Betto.Services.Services
{
    public interface ITicketService
    {
        Task<RequestResponse<TicketDTO>> GetTicketByIdAsync(int ticketId);
        Task<RequestResponse<IEnumerable<TicketDTO>>> GetUserTicketsAsync(int userId);
        Task<RequestResponse<TicketDTO>> AddTicketAsync(CreateTicketDTO ticket);
        Task<RequestResponse<TicketDTO>> RevealTicketAsync(int ticketId);
    }
}
