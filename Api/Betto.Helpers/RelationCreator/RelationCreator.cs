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

        private void PrepareLeaguesToSave(IList<LeagueEntity> leagues, IList<TeamEntity> teams, IList<GameEntity> games)
        {
            foreach (var league in leagues.GetEmptyIfNull())
            {
                league.Teams = teams.Where(t => t.LeagueId == league.LeagueId).ToList();
                league.Games = games.Where(g => g.LeagueId == league.LeagueId).ToList();
                league.LeagueId = 0;
            }
        }

        private void PrepareTeamsToSave(IList<TeamEntity> teams, IList<GameEntity> games)
        {
            foreach (var team in teams.GetEmptyIfNull())
            {
                team.HomeGames = games.Where(g => g.HomeTeam.TeamId == team.TeamId).ToList();
                team.AwayGames = games.Where(g => g.AwayTeam.TeamId == team.TeamId).ToList();
                team.TeamId = 0;
            }
        }

        private void PrepareGamesToSave(IList<GameEntity> games)
        {
            foreach (var game in games.GetEmptyIfNull())
            {
                game.GameId = 0;
                game.HomeTeam = null; //avoid repetitions in entity tracking
                game.AwayTeam = null;
            }
        }
    }
}
