using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.ViewModels;
using Betto.Model.Models;
using Betto.Model.WriteModels;

namespace Betto.Services.Services
{
    public interface ITicketService
    {
        Task<RequestResponse<TicketViewModel>> GetTicketByIdAsync(int ticketId);
        Task<RequestResponse<IEnumerable<TicketViewModel>>> GetUserTicketsAsync(int userId);
        Task<RequestResponse<TicketViewModel>> AddTicketAsync(TicketWriteModel ticket);
        Task<RequestResponse<TicketViewModel>> RevealTicketAsync(int ticketId);
    }
}
