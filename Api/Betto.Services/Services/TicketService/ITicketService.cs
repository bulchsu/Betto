using System.Threading.Tasks;
using Betto.Model.ViewModels;
using Betto.Model.Models;
using Betto.Model.WriteModels;

namespace Betto.Services.Services
{
    public interface ITicketService
    {
        Task<RequestResponseModel<TicketViewModel>> GetTicketByIdAsync(int ticketId);
        Task<RequestResponseModel<TicketViewModel>> AddTicketAsync(TicketWriteModel ticket);
        Task<RequestResponseModel<TicketViewModel>> RevealTicketAsync(int ticketId);
    }
}
