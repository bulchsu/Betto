using Betto.DataAccessLayer.LeagueRepository.Repositories;
using Betto.Helpers;
using Betto.Helpers.LeagueParser;
using Betto.Model.Entities;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Services.Services.ImportService
{
    public class ImportService : IImportService
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly ILeagueParser _leagueParser;

        public ImportService(ILeagueRepository leagueRepository, ILeagueParser leagueParser)
        {
            this._leagueRepository = leagueRepository;
            this._leagueParser = leagueParser;
        }

        public async Task ImportExternalDataAsync()
        {   
            _leagueRepository.Clear();

            var leagues = await _leagueParser.GetLeaguesAsync();

            await _leagueRepository.AddLeaguesAsync(leagues);
            await _leagueRepository.SaveChangesAsync();
        }
    }
}
