using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<BetRatesEntity> GetGameRatesAsync(int gameId) =>
            await DbContext.Rates.SingleOrDefaultAsync(r => r.GameId == gameId);

        public void Clear()
            => DbContext.Rates.RemoveRange(DbContext.Rates);
    }
}
