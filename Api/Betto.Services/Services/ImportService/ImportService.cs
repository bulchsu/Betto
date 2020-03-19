using Betto.DataAccessLayer.LeagueRepository.Repositories;
using Betto.Helpers;
using Betto.Helpers.JSONManager;
using Betto.Model.Entities;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Services.Services.ImportService
{
    public class ImportService : IImportService
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly IOptions<RapidApiConfiguration> _apiConfiguration;

        public ImportService(ILeagueRepository leagueRepository, IOptions<RapidApiConfiguration> apiConfiguration)
        {
            this._leagueRepository = leagueRepository;
            this._apiConfiguration = apiConfiguration;
        }

        public async Task ImportExternalDataAsync()
        {   
            _leagueRepository.Clear();

            var leagues = await GetLeaguesAsync();

            await _leagueRepository.AddLeaguesAsync(leagues);
            await _leagueRepository.SaveChangesAsync();
        }

        private async Task<IEnumerable<LeagueEntity>> GetLeaguesAsync()
        {
            var jsonManager = new JSONManager(_apiConfiguration.Value);
            var leagues = await jsonManager.GetLeaguesAsync();

            return leagues;
        }
    }
}
