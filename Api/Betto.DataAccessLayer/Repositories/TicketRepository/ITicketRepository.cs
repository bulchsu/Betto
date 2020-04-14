using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Entities;

namespace Betto.DataAccessLayer.Repositories
{
    public interface ITicketRepository : IBaseRepository
    {
        Task<IEnumerable<TicketEntity>> GetUserTicketsAsync(int userId);
        Task<TicketEntity> AddTicketAsync(TicketEntity ticket);
        Task<TicketEntity> GetTicketByIdAsync(int ticketId);
    }
}
