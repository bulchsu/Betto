using System;
using Betto.DataAccessLayer.Repositories;
using Betto.Model.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.Model.Constants;
using Betto.Resources.Shared;
using Microsoft.Extensions.Localization;

namespace Betto.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly ILeagueRepository _leagueRepository;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public LeagueService(ILeagueRepository leagueRepository, IStringLocalizer<ErrorMessages> localizer)
        {
            _leagueRepository = leagueRepository;
            _localizer = localizer;
        }

        public async Task<LeagueTableDTO> GetLeagueTableAsync(int leagueId)
        {
            var league = await _leagueRepository.GetLeagueByIdAsync(leagueId, true, true);

            if (league.Type == LeagueConstants.CupTypeString)
            {
                throw new Exception(_localizer["CantCreateCupTableErrorMessage", league.Name].Value);
            }

            return LeagueTableDTO.Factory.NewLeagueTable(league);
        }

        public async Task<LeagueDTO> GetLeagueByIdAsync(int leagueId, bool includeTeams, bool includeGames)
            => (LeagueDTO)(await _leagueRepository.GetLeagueByIdAsync(leagueId, includeTeams, includeGames));

        public async Task<IEnumerable<LeagueDTO>> GetLeaguesAsync()
            => (await _leagueRepository.GetLeaguesAsync()).Select(l => (LeagueDTO)l);
    }
}
