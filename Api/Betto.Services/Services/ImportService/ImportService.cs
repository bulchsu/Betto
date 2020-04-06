using Betto.DataAccessLayer.Repositories;
using Betto.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.RapidApiCommunication.Managers;

namespace Betto.Services
{
    public class ImportService : IImportService
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly ILeagueManager _leagueManager;
        private readonly ITeamManager _teamManager;

        public ImportService(ILeagueRepository leagueRepository, ILeagueManager leagueManager, ITeamManager teamManager)
        {
            this._leagueRepository = leagueRepository;
            this._leagueManager = leagueManager;
            this._teamManager = teamManager;
        }

        public async Task ImportExternalDataAsync()
        {   
            _leagueRepository.Clear();

            var leaguesBeforeCombination = await _leagueManager.GetLeaguesAsync();
            var teams = await _teamManager.GetTeamsAsync();

            var leagues = CombineLeaguesWithTeamsAsync(leaguesBeforeCombination, teams);

            await _leagueRepository.AddLeaguesAsync(leagues);
            await _leagueRepository.SaveChangesAsync();
        }

        private IEnumerable<LeagueEntity> CombineLeaguesWithTeamsAsync(IEnumerable<LeagueEntity> leagues, IEnumerable<IEnumerable<TeamEntity>> teams)
        {
            if (leagues != null && teams != null)
            {
                for (var i = 0; i < leagues.Count(); i++)
                {
                    leagues.ElementAt(i).Teams = teams.ElementAt(i).ToList();
                }
            }

            return leagues;
        }
    }
}
