using Betto.Model.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Helpers
{
    public interface ITeamParser
    {
        Task<IEnumerable<IEnumerable<TeamEntity>>> GetTeamsAsync();
    }
}
