using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.DTO;

namespace Betto.Services.Services
{
    public interface ITicketService
    {
        Task<TicketDTO> AddTicketAsync(CreateTicketDTO ticket);
        Task<ICollection<TicketDTO>> GetUserTicketsAsync(int userId);
        Task<TicketDTO> GetTicketByIdAsync(int ticketId);
        Task<bool> CheckHasUserPlayedAnyOfGamesBeforeAsync(CreateTicketDTO ticket);
    }
}
