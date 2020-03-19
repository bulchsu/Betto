using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories.BaseRepository
{
    public abstract class BaseRepository : IBaseRepository
    {

        protected readonly BettoDbContext DbContext;

        public BaseRepository(BettoDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync() 
            => await DbContext.SaveChangesAsync();

    }
}
