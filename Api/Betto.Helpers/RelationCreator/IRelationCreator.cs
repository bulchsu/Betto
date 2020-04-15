using System.Collections.Generic;
using Betto.Model.Entities;

namespace Betto.Helpers
{
    public interface IRelationCreator
    {
        void RelateImportedData(ICollection<LeagueEntity> leagues, ICollection<TeamEntity> teams, ICollection<GameEntity> games);
    }
}
