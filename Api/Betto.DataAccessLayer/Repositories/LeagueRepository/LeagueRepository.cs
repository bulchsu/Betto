using Betto.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.DataAccessLayer.Repositories.BaseRepository;

namespace Betto.DataAccessLayer.LeagueRepository.Repositories
{
    public class LeagueRepository : BaseRepository, ILeagueRepository
    {
        public LeagueRepository(BettoDbContext dbContext) 
            : base(dbContext)
        {

        }

        public async Task AddLeaguesAsync(IEnumerable<LeagueEntity> leagues) 
            => await DbContext.AddRangeAsync(leagues);

        public void Clear()
            => DbContext.Leagues.RemoveRange(DbContext.Leagues);

        public async Task<LeagueEntity> GetLeagueByIdAsync(int leagueId)
            => await DbContext.Leagues.SingleOrDefaultAsync(l => l.LeagueId == leagueId);

        public async Task<IEnumerable<LeagueEntity>> GetLeaguesAsync()
            => await DbContext.Leagues.ToListAsync();
    }
}
