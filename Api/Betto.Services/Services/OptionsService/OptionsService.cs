using System.Collections.Concurrent;
using Betto.DataAccessLayer.Repositories;
using Betto.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.Configuration;
using Betto.DataAccessLayer.Repositories.RatesRepository;
using Betto.Helpers;
using Betto.Helpers.Extensions;
using Betto.RapidApiCommunication.Managers;
using Microsoft.Extensions.Options;

namespace Betto.Services
{
    public class OptionsService : IOptionsService
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly IRatesRepository _rateRepository;
        private readonly ILeagueManager _leagueManager;
        private readonly ITeamManager _teamManager;
        private readonly IGameManager _gameManager;
        private readonly IRelationCreator _relationCreator;
        private readonly IRateCalculator _rateCalculator;
        private readonly RapidApiConfiguration _configuration;

        public OptionsService(ILeagueRepository leagueRepository,
            IRatesRepository rateRepository,
            ILeagueManager leagueManager,
            ITeamManager teamManager,
            IGameManager gameManager,
            IRelationCreator relationCreator,
            IRateCalculator rateCalculator,
            IOptions<RapidApiConfiguration> configuration)
        {
            _leagueRepository = leagueRepository;
            _rateRepository = rateRepository;
            _leagueManager = leagueManager;
            _teamManager = teamManager;
            _gameManager = gameManager;
            _relationCreator = relationCreator;
            _rateCalculator = rateCalculator;
            _configuration = configuration.Value;
        }

        public async Task ImportInitialDataAsync()
        {   
            _leagueRepository.Clear();

            var leagueIds = CalculateLeaguesIds(_configuration.LeaguesAmount).ToList();
            var leagues = await RetrieveLeaguesAsync(leagueIds);

            await SaveLeaguesAsync(leagues);
        }

        public async Task ImportNextLeaguesAsync(int leaguesAmount)
        {
            var highestStoredLeagueId = (await _leagueRepository.GetLeaguesAsync(false, false)).Max(l => l.RapidApiExternalId);
            var leagueIds = CalculateLeaguesIds(leaguesAmount, highestStoredLeagueId).ToList();

            var leagues = await RetrieveLeaguesAsync(leagueIds);

            await SaveLeaguesAsync(leagues);
        }

        public async Task SetBetRatesForAllLeaguesAsync()
        {
            _rateRepository.Clear();
            var leagues = await _leagueRepository.GetLeaguesAsync(true, true);
            
            await SetRatesForLeagues(leagues);
        }

        public async Task SetBetRatesForAdditionalLeaguesAsync(int leaguesAmount)
        {
            var leagues = (await _leagueRepository.GetLeaguesAsync(true, true)).TakeLast(leaguesAmount);
            await SetRatesForLeagues(leagues);
        }

        private async Task SetRatesForLeagues(IEnumerable<LeagueEntity> leagues)
        {
            var rates = new ConcurrentBag<BetRatesEntity>();

            Parallel.ForEach(leagues, league =>
            {
                var leagueRates = _rateCalculator.GetLeagueGamesRates(league);
                rates.AddRange(leagueRates);
            });

            await SaveRatesAsync(rates.ToList());
        }

        private async Task<IList<LeagueEntity>> RetrieveLeaguesAsync(ICollection<int> leaguesToImportIds)
        {
            IList<LeagueEntity> leagues = (await _leagueManager.GetLeaguesAsync(leaguesToImportIds)).ToList();
            var teams = (await _teamManager.GetTeamsAsync(leaguesToImportIds)).ToList();
            var games = (await _gameManager.GetGamesAsync(leaguesToImportIds)).ToList();

            _relationCreator.RelateImportedData(leagues, teams, games);

            return leagues;
        }

        private IEnumerable<int> CalculateLeaguesIds(int leaguesToImportAmount, int highestStoredLeagueId = 0)
        {
            var leagueId = ++highestStoredLeagueId;

            for (var i = 0; i < leaguesToImportAmount; i++)
            {
                yield return leagueId;
                ++leagueId;
            }
        }

        private async Task<int> SaveLeaguesAsync(ICollection<LeagueEntity> leagues)
        {
            await _leagueRepository.AddLeaguesAsync(leagues);
            return await _leagueRepository.SaveChangesAsync();
        }

        private async Task<int> SaveRatesAsync(ICollection<BetRatesEntity> rates)
        {
            await _rateRepository.AddRatesAsync(rates);
            return await _rateRepository.SaveChangesAsync();
        }
    }
}
