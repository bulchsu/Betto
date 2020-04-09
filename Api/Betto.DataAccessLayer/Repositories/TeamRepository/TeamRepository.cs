using Betto.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories
{
    public class TeamRepository : BaseRepository, ITeamRepository
    {
        public TeamRepository(BettoDbContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<IEnumerable<TeamEntity>> GetLeagueTeamsAsync(int leagueId)
            => await DbContext.Teams
            .Where(t => t.LeagueId == leagueId)
            .ToListAsync();

        public async Task<TeamEntity> GetTeamByIdAsync(int id, bool includeVenue)
        {
            IQueryable<TeamEntity> query = DbContext.Teams;

            if (includeVenue)
            {
                query = query.Include(t => t.Venue);
            }

            return await query.SingleOrDefaultAsync(t => t.TeamId == id);
        }
    }
}
