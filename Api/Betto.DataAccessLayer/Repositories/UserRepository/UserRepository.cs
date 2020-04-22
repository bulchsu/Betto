using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Betto.DataAccessLayer.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(BettoDbContext dbContext) 
            : base(dbContext)
        {

        }

        public async Task<UserEntity> SignUpAsync(UserEntity newUser) => 
            (await DbContext.Users.AddAsync(newUser)).Entity;

        public async Task<UserEntity> GetUserByUsernameAsync(string username) =>
            await DbContext.Users.SingleOrDefaultAsync(u => u.Username == username);

        public async Task<UserEntity> GetUserByMailAsync(string mailAddress) =>
            await DbContext.Users.SingleOrDefaultAsync(u => u.MailAddress == mailAddress);

        public async Task<UserEntity> GetUserByIdAsync(int userId,
            bool includePayments, bool includeTickets)
        {
            IQueryable<UserEntity> query = DbContext.Users;

            if (includePayments)
            {
                query = query.Include(u => u.Payments);
            }

            if (includeTickets)
            {
                query = query.Include(u => u.Tickets);
            }

            return await query
                .SingleOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<ICollection<UserEntity>> GetUsersAsync() =>
            await DbContext.Users.ToListAsync();
    }
}
