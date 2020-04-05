using Betto.DataAccessLayer.Repositories;
using Betto.Helpers;
using Betto.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Betto.Services.Services
{
    public class ImportService : IImportService
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly ILeagueParser _leagueParser;
        private readonly ITeamParser _teamParser;

        public ImportService(ILeagueRepository leagueRepository, ILeagueParser leagueParser, ITeamParser teamParser)
        {
            this._leagueRepository = leagueRepository;
            this._leagueParser = leagueParser;
            this._teamParser = teamParser;
        }

        public async Task ImportExternalDataAsync()
        {   
            _leagueRepository.Clear();

            var leaguesBeforeCombination = await _leagueParser.GetLeaguesAsync();
            var teams = await _teamParser.GetTeamsAsync();

            var leagues = CombineLeaguesWithTeamsAsync(leaguesBeforeCombination, teams);

            await _leagueRepository.AddLeaguesAsync(leagues);
            await _leagueRepository.SaveChangesAsync();
        }

        private IEnumerable<LeagueEntity> CombineLeaguesWithTeamsAsync(IEnumerable<LeagueEntity> leagues, IEnumerable<IEnumerable<TeamEntity>> teams)
        {
            for (int i = 0; i < leagues.Count(); i++)
            {
                leagues.ElementAt(i).Teams = teams.ElementAt(i).ToList();
            }

            return leagues;
        }
    }
}
