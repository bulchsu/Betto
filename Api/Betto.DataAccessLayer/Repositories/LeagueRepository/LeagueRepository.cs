using Betto.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories
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

        public async Task<LeagueEntity> GetLeagueByIdAsync(int leagueId, bool includeTeams, bool includeGames) =>
            await PrepareGetLeaguesQuery(includeTeams, includeGames)
                .SingleOrDefaultAsync(l => l.LeagueId == leagueId);


        public async Task<IEnumerable<LeagueEntity>> GetLeaguesAsync(bool includeTeams, bool includeGames) =>
            await PrepareGetLeaguesQuery(includeTeams, includeGames)
                .ToListAsync();

        private IQueryable<LeagueEntity> PrepareGetLeaguesQuery(bool includeTeams, bool includeGames)
        {
            IQueryable<LeagueEntity> query = DbContext.Leagues;

            if (includeTeams)
            {
                query = query.Include(l => l.Teams);
            }
            if (includeGames)
            {
                query = query.Include(l => l.Games);
            }

            return query;
        }
    }
}
