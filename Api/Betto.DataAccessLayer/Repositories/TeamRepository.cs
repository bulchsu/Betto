using Betto.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories
{
    public class TeamRepository : BaseRepository, ITeamRepository
    {

        public TeamRepository(BettoDbContext dbContext) : base(dbContext)
        {

        }

        public async Task AddLeagueTeamsAsync(IEnumerable<TeamEntity> teams) => await DbContext.AddRangeAsync(teams);
   
    }
}
