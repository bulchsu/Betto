using System.Collections.Generic;
using Betto.Model.Entities;

namespace Betto.Helpers
{
    public interface IRelationCreator
    {
        void RelateImportedData(IList<LeagueEntity> leagues, IList<TeamEntity> teams, IList<GameEntity> games);
    }
}
