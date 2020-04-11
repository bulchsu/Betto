using Betto.DataAccessLayer.Repositories;
using Betto.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.Configuration;
using Betto.Helpers;
using Betto.RapidApiCommunication.Managers;
using Microsoft.Extensions.Options;

namespace Betto.Services
{
    public class ImportService : IImportService
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly ILeagueManager _leagueManager;
        private readonly ITeamManager _teamManager;
        private readonly IGameManager _gameManager;
        private readonly IRelationCreator _relationCreator;
        private readonly RapidApiConfiguration _configuration;

        public ImportService(ILeagueRepository leagueRepository,
            ILeagueManager leagueManager,
            ITeamManager teamManager,
            IGameManager gameManager,
            IRelationCreator relationCreator,
            IOptions<RapidApiConfiguration> configuration)
        {
            _leagueRepository = leagueRepository;
            _leagueManager = leagueManager;
            _teamManager = teamManager;
            _gameManager = gameManager;
            _relationCreator = relationCreator;
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
            var highestStoredLeagueId = (await _leagueRepository.GetLeaguesAsync()).Max(l => l.RapidApiExternalId);
            var leagueIds = CalculateLeaguesIds(leaguesAmount, highestStoredLeagueId).ToList();

            var leagues = await RetrieveLeaguesAsync(leagueIds);

            await SaveLeaguesAsync(leagues);
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
    }
}
