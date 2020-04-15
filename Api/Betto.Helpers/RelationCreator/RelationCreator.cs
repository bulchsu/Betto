using System.Collections.Generic;
using System.Linq;
using Betto.Helpers.Extensions;
using Betto.Model.Entities;

namespace Betto.Helpers
{
    public class RelationCreator : IRelationCreator
    {
        public void RelateImportedData(IList<LeagueEntity> leagues, IList<TeamEntity> teams,
            IList<GameEntity> games)
        {
            PrepareLeaguesToSave(leagues, teams, games);
            PrepareTeamsToSave(teams, games);
            PrepareGamesToSave(games);
        }

        private static void PrepareLeaguesToSave(ICollection<LeagueEntity> leagues, ICollection<TeamEntity> teams, ICollection<GameEntity> games)
        {
            foreach (var league in leagues.GetEmptyIfNull())
            {
                league.Teams = teams.Where(t => t.LeagueId == league.RapidApiExternalId).ToList();
                league.Games = games.Where(g => g.LeagueId == league.RapidApiExternalId).ToList();
            }
        }

        private static void PrepareTeamsToSave(ICollection<TeamEntity> teams, ICollection<GameEntity> games)
        {
            foreach (var team in teams.GetEmptyIfNull())
            {
                team.HomeGames = games.Where(g => g.HomeTeam.RapidApiExternalId == team.RapidApiExternalId).ToList();
                team.AwayGames = games.Where(g => g.AwayTeam.RapidApiExternalId == team.RapidApiExternalId).ToList();
            }
        }

        private static void PrepareGamesToSave(ICollection<GameEntity> games) //avoid repetitions in entity tracking
        {
            foreach (var game in games.GetEmptyIfNull())
            {
                game.HomeTeam = null; 
                game.AwayTeam = null;
            }
        }
    }
}
