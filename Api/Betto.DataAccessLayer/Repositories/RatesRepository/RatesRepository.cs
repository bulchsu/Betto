using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Entities;

namespace Betto.DataAccessLayer.Repositories.RatesRepository
{
    public class RatesRepository : BaseRepository, IRatesRepository
    {
        public RatesRepository(BettoDbContext dbContext) 
            : base(dbContext)
        {
        }

        public async Task AddRatesAsync(ICollection<BetRatesEntity> rates) =>
            await DbContext.AddRangeAsync(rates);

        public void Clear()
            => DbContext.Rates.RemoveRange(DbContext.Rates);
    }
}
