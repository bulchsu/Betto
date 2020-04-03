using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    { 
        protected readonly BettoDbContext DbContext;

        protected BaseRepository(BettoDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync()
            => await DbContext.SaveChangesAsync();
    }
}
