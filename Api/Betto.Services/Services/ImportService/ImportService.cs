using Betto.DataAccessLayer.Repositories;
using Betto.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.Helpers;
using Betto.RapidApiCommunication.Managers;

namespace Betto.Services
{
    public class ImportService : IImportService
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly ILeagueManager _leagueManager;
        private readonly ITeamManager _teamManager;
        private readonly IGameManager _gameManager;
        private readonly IRelationCreator _relationCreator;

        public ImportService(ILeagueRepository leagueRepository,
            ILeagueManager leagueManager,
            ITeamManager teamManager,
            IGameManager gameManager,
            IRelationCreator relationCreator)
        {
            _leagueRepository = leagueRepository;
            _leagueManager = leagueManager;
            _teamManager = teamManager;
            _gameManager = gameManager;
            _relationCreator = relationCreator;
        }

        public async Task ImportExternalDataAsync()
        {   
            _leagueRepository.Clear();

            IList<LeagueEntity> leagues = (await _leagueManager.GetLeaguesAsync()).ToList();
            var leagueIds = leagues.Select(l => l.LeagueId).ToList();

            var teams = (await _teamManager.GetTeamsAsync(leagueIds)).ToList();
            var games = (await _gameManager.GetGamesAsync(leagueIds)).ToList();

            _relationCreator.RelateImportedData(leagues, teams, games);

            await _leagueRepository.AddLeaguesAsync(leagues);
            await _leagueRepository.SaveChangesAsync();
        }
    }
}
