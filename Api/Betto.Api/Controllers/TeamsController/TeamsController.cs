using Betto.Model.DTO;
using Betto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Api.Text;
using Microsoft.Extensions.Localization;

namespace Betto.Api.Controllers
{
    [ApiController]
    [Route("api/leagues/{leagueId}/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ILeagueService _leagueService;
        private readonly ITeamService _teamService;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public TeamsController(ILeagueService leagueService, ITeamService teamService, IStringLocalizer<ErrorMessages> localizer)
        {
            this._leagueService = leagueService;
            this._teamService = teamService;
            this._localizer = localizer;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetLeagueTeamsAsync(int leagueId)
        {
            try
            {
                var teams = await _teamService.GetLeagueTeamsAsync(leagueId);

                if (teams == null)
                {
                    return NotFound(new { Message = _localizer["LackOfTeamsErrorMessage"].Value });
                }

                return Ok(teams);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = ex.InnerException != null
                            ? $"{ex.Message} {ex.InnerException.Message}"
                            : ex.Message
                    });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TeamDTO>> GetTeamByIdAsync(int leagueId, int id, bool includeVenue = false)
        {
            try
            {
                var league = await _leagueService.GetLeagueByIdAsync(leagueId);

                if (league == null)
                {
                    return NotFound(new { Message = _localizer["LeagueNotFoundErrorMessage"].Value });
                }

                var team = await _teamService.GetTeamByIdAsync(id, includeVenue);

                if (team == null)
                {
                    return NotFound(new { Message = _localizer["TeamNotFoundErrorMessage"].Value });
                }

                return Ok(team);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = ex.InnerException != null
                            ? $"{ex.Message} {ex.InnerException.Message}"
                            : ex.Message
                    });
            }
        }
    }
}
