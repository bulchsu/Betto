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
                var statistics = GenerateLeagueTeamsStatistics(league)
                    .OrderByDescending(t => t.Points);

                var queue = new Queue<TeamStatistics>(statistics);

                SetTablePositions(queue);

                return queue;
            }

            private static IEnumerable<TeamStatistics> GenerateLeagueTeamsStatistics(LeagueEntity league)
            {
                foreach (var team in league.Teams)
                {
                    var teamHomeGames = league.Games.Where(g => g.HomeTeamId == team.TeamId);
                    var teamAwayGames = league.Games.Where(g => g.AwayTeamId == team.TeamId);

                    var homeGamesWon = teamHomeGames.Count(g => g.GoalsHomeTeam > g.GoalsAwayTeam);
                    var homeGamesLost = teamHomeGames.Count(g => g.GoalsHomeTeam < g.GoalsAwayTeam);
                    var homeGamesTied = teamHomeGames.Count(g => g.GoalsHomeTeam == g.GoalsAwayTeam);

                    var awayGamesWon = teamAwayGames.Count(g => g.GoalsHomeTeam < g.GoalsAwayTeam);
                    var awayGamesLost = teamAwayGames.Count(g => g.GoalsHomeTeam > g.GoalsAwayTeam);
                    var awayGamesTied = teamAwayGames.Count(g => g.GoalsHomeTeam == g.GoalsAwayTeam);

                    var goalsScored = teamHomeGames.Sum(t => t.GoalsHomeTeam) + teamAwayGames.Sum(t => t.GoalsAwayTeam);
                    var goalsLost = teamHomeGames.Sum(t => t.GoalsAwayTeam) + teamAwayGames.Sum(t => t.GoalsHomeTeam);
                    var points = (homeGamesWon + awayGamesWon) * GameConstants.WonGamePointsAmount +
                                 (homeGamesTied + awayGamesTied) * GameConstants.TiedGamePointsAmount;

                    var teamStatistics = new TeamStatistics
                    {
                        TeamId = team.TeamId,
                        TeamName = team.Name,
                        Points = points,
                        GoalsScored = goalsScored,
                        GoalsLost = goalsLost,
                        WonGamesAmount = homeGamesWon + awayGamesWon,
                        TiedMatchesAmount = homeGamesTied + awayGamesTied,
                        LostGamesAmount = homeGamesLost + awayGamesLost
                    };

                    yield return teamStatistics;
                }
            }

            private static void SetTablePositions(Queue<TeamStatistics> teamStatistics)
            {
                var position = 1;

                foreach (var team in teamStatistics)
                {
                    team.Position = position;
                    ++position;
                }
            }
        }
    }
}
