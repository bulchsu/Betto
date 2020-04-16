using System.Collections.Generic;
using System.Linq;
using Betto.Model.Constants;
using Betto.Model.Entities;

namespace Betto.Model.Models
{
    public partial class LeagueTable
    {
        public static class Factory
        {
            public static LeagueTable NewLeagueTable(LeagueEntity league)
            {
                var table = SortOutLeagueTeams(league);
                return new LeagueTable(league.LeagueId, league.Name, table);
            }

            private static Queue<TeamStatistics> SortOutLeagueTeams(LeagueEntity league)
            {
                var statistics = GenerateLeagueTeamsStatistics(league);

                var queue = new Queue<TeamStatistics>(statistics);

                SetTablePositions(queue);

                return queue;
            }

            private static IEnumerable<TeamStatistics> GenerateLeagueTeamsStatistics(LeagueEntity league)
            {
                return (from team in league.Teams
                    let teamHomeGames = league.Games.Where(g => g.HomeTeamId == team.TeamId)
                    let teamAwayGames = league.Games.Where(g => g.AwayTeamId == team.TeamId)
                    let homeGamesWon = teamHomeGames.Count(g => g.GoalsHomeTeam > g.GoalsAwayTeam)
                    let homeGamesLost = teamHomeGames.Count(g => g.GoalsHomeTeam < g.GoalsAwayTeam)
                    let homeGamesTied = teamHomeGames.Count(g => g.GoalsHomeTeam == g.GoalsAwayTeam)
                    let awayGamesWon = teamAwayGames.Count(g => g.GoalsHomeTeam < g.GoalsAwayTeam)
                    let awayGamesLost = teamAwayGames.Count(g => g.GoalsHomeTeam > g.GoalsAwayTeam)
                    let awayGamesTied = teamAwayGames.Count(g => g.GoalsHomeTeam == g.GoalsAwayTeam)
                    let goalsScored = teamHomeGames.Sum(t => t.GoalsHomeTeam) + teamAwayGames.Sum(t => t.GoalsAwayTeam)
                    let goalsLost = teamHomeGames.Sum(t => t.GoalsAwayTeam) + teamAwayGames.Sum(t => t.GoalsHomeTeam)
                    let points = (homeGamesWon + awayGamesWon) * GameConstants.WonGamePointsAmount +
                                 (homeGamesTied + awayGamesTied) * GameConstants.TiedGamePointsAmount 
                    select new TeamStatistics
                {
                    TeamId = team.TeamId,
                    TeamName = team.Name,
                    Points = points,
                    GoalsScored = goalsScored,
                    GoalsLost = goalsLost,
                    WonGamesAmount = homeGamesWon + awayGamesWon,
                    TiedMatchesAmount = homeGamesTied + awayGamesTied,
                    LostGamesAmount = homeGamesLost + awayGamesLost
                })
                    .OrderByDescending(t => t.Points)
                    .ToList();
            }

            private static void SetTablePositions(IEnumerable<TeamStatistics> ranking)
            {
                var position = 1;

                foreach (var rank in ranking)
                {
                    rank.Position = position;
                    ++position;
                }
            }
        }
    }
}
