using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Betto.DataAccessLayer.Repositories
{
    public class TicketRepository : BaseRepository, ITicketRepository
    {
        public TicketRepository(BettoDbContext dbContext) 
            : base(dbContext)
        {

        }

        public async Task<IEnumerable<TicketEntity>> GetUserTicketsAsync(int userId) =>
            await DbContext.Tickets
                .Include(t => t.Events)
                .Where(t => t.UserId == userId)
                .ToListAsync();

        public async Task<TicketEntity> AddTicketAsync(TicketEntity ticket) =>
            (await DbContext.Tickets.AddAsync(ticket)).Entity;

        public async Task<TicketEntity> GetTicketByIdAsync(int ticketId) =>
            await DbContext.Tickets
                .Include(t => t.Events)
                .SingleOrDefaultAsync(t => t.TicketId == ticketId);
    }
}
