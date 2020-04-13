using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Helpers.Extensions;
using Betto.Model.Constants;
using Betto.Model.Entities;
using Betto.Model.Models;

namespace Betto.Helpers
{
    public class RateCalculator : IRateCalculator
    {
        public ICollection<BetRatesEntity> GetLeagueGamesRates(LeagueEntity league)
        {
            var leagueTable = LeagueTable.Factory.NewLeagueTable(league);
            var betRates = new ConcurrentBag<BetRatesEntity>();

            Parallel.ForEach(league.Games.GetEmptyIfNull(), game =>
            {
                var homeTeam = leagueTable.Table.SingleOrDefault(t => t.TeamId == game.HomeTeamId);
                var awayTeam = leagueTable.Table.SingleOrDefault(t => t.TeamId == game.AwayTeamId);

                var rates = GetGameRates(homeTeam, awayTeam, league.Teams.Count);
                rates.GameId = game.GameId;

                betRates.Add(rates);
            });

            return betRates.ToList();
        }

        private BetRatesEntity GetGameRates(TeamStatistics homeTeam, TeamStatistics awayTeam, int leagueSize)
        {
            var positionDifference = GetPositionDifference(homeTeam, awayTeam);
            var probabilityInitialFactor = CalculateInitialProbabilityFactor(positionDifference);

            var initialProbability = GetInitialProbability(homeTeam, awayTeam, probabilityInitialFactor);
            var randomExtraProbability = GetRandomExtraProbability(positionDifference, leagueSize);

            var homeTeamWinProbability =
                initialProbability.HomeTeamWinFactor + randomExtraProbability.HomeTeamWinFactor;
            var awayTeamWinProbability =
                initialProbability.AwayTeamWinFactor + randomExtraProbability.AwayTeamWinFactor;
            var tieProbability = 
                initialProbability.TieFactor + randomExtraProbability.TieFactor;

            homeTeamWinProbability = InverseNumber(homeTeamWinProbability);
            awayTeamWinProbability = InverseNumber(awayTeamWinProbability);
            tieProbability = InverseNumber(tieProbability);

            homeTeamWinProbability = SubtractGamblingCompanyFactor(homeTeamWinProbability);
            awayTeamWinProbability = SubtractGamblingCompanyFactor(awayTeamWinProbability);
            tieProbability = SubtractGamblingCompanyFactor(tieProbability);

            return new BetRatesEntity
            {
                HomeTeamWinRate = (float)Math.Round(homeTeamWinProbability, 2),
                AwayTeamWinRate = (float)Math.Round(awayTeamWinProbability, 2),
                TieRate = (float)Math.Round(tieProbability, 2)
            };
        }

        private ProbabilityFactors GetInitialProbability(TeamStatistics homeTeam, TeamStatistics awayTeam, float probabilityInitialFactor)
        {
            var homeTeamWinInitialProbability = RatesConstants.WinInitialProbability + (homeTeam.Position < awayTeam.Position ? probabilityInitialFactor : -probabilityInitialFactor);
            var awayTeamWinInitialProbability = RatesConstants.WinInitialProbability + (homeTeam.Position > awayTeam.Position ? probabilityInitialFactor : -probabilityInitialFactor);
            var tieInitialProbability = RatesConstants.CertainEventProbability - (homeTeamWinInitialProbability + awayTeamWinInitialProbability);

            return new ProbabilityFactors
            {
                HomeTeamWinFactor = homeTeamWinInitialProbability,
                AwayTeamWinFactor = awayTeamWinInitialProbability,
                TieFactor = tieInitialProbability
            };
        }

        private ProbabilityFactors GetRandomExtraProbability(int positionDifference, int leagueSize)
        {
            var randomGenerator = new Random();
            var pointsToAllocate = leagueSize - positionDifference;
            var randomBaseValue = (float)randomGenerator.Next(pointsToAllocate);
            var randomDependentValue = randomBaseValue / 2.0f;

            var winExtraProbability =
                (float) randomGenerator.NextDouble() * RatesConstants.WinInitialProbabilityVariation;

            var luckFactor =
                (float) (randomGenerator.NextDouble() * (RatesConstants.MaximumLuckFactor - RatesConstants.MinimumLuckFactor) +
                RatesConstants.MinimumLuckFactor);

            var homeTeamExtraProbability = randomBaseValue * luckFactor;
            var awayTeamExtraProbability = -randomDependentValue * luckFactor;
            var tieExtraProbability = awayTeamExtraProbability;

            homeTeamExtraProbability += winExtraProbability;
            awayTeamExtraProbability += winExtraProbability;
            tieExtraProbability -= (2.0f * winExtraProbability);

            return new ProbabilityFactors
            {
                HomeTeamWinFactor = homeTeamExtraProbability,
                AwayTeamWinFactor = awayTeamExtraProbability,
                TieFactor = tieExtraProbability
            };
        }

        private float CalculateInitialProbabilityFactor(int positionDifference) =>
            RatesConstants.PositionDifferenceProbabilityFactor * positionDifference;

        private int GetPositionDifference(TeamStatistics homeTeamStatistics, TeamStatistics awayTeamStatistics) =>
            Math.Abs(homeTeamStatistics.Position - awayTeamStatistics.Position);

        private float SubtractGamblingCompanyFactor(float value) =>
            RatesConstants.GamblingCompanyProbabilityFactor * value > RatesConstants.MinimumCalculatedProbability
                ? RatesConstants.GamblingCompanyProbabilityFactor * value
                : RatesConstants.MinimumCalculatedProbability;

        private float InverseNumber(float value) =>
            (float)Math.Pow(value, -1);
    }
}
