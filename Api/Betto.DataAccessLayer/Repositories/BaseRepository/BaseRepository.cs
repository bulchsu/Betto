using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories
{
    public abstract class BaseRepository
    { 
        protected readonly BettoDbContext DbContext;

        protected BaseRepository(BettoDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual async Task<int> SaveChangesAsync()
            => await DbContext.SaveChangesAsync();
    }
}
