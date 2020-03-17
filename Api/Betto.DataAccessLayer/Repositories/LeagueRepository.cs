using Betto.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories
{
    public class LeagueRepository : BaseRepository, ILeagueRepository
    {

        public LeagueRepository(BettoDbContext dbContext) : base(dbContext)
        {

        }

        public async Task AddLeaguesAsync(IEnumerable<LeagueEntity> leagues) 
            => await DbContext.AddRangeAsync(leagues);

        public void Clear()
            => DbContext.Leagues.RemoveRange(DbContext.Leagues);

    }
}
