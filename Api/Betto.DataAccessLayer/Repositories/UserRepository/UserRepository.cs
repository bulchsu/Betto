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

        public async Task<UserEntity> GetUserByIdAsync(int userId) =>
            await DbContext.Users.SingleOrDefaultAsync(u => u.UserId == userId);
    }
}
