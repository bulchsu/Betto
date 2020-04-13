using Betto.Model.DTO;
using Betto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Resources.Shared;
using Microsoft.Extensions.Localization;

namespace Betto.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaguesController : ControllerBase
    {
        private readonly ILeagueService _leagueService;
        private readonly ITeamService _teamService;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public LeaguesController(ILeagueService leagueService, 
            ITeamService teamService, 
            IStringLocalizer<ErrorMessages> localizer)
        {
            _leagueService = leagueService;
            _teamService = teamService;
            _localizer = localizer;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeagueDTO>>> GetLeaguesAsync([FromQuery] bool includeTeams = false, [FromQuery] bool includeGames = false)
        {
            try
            {
                var leagues = await _leagueService.GetLeaguesAsync(includeTeams, includeGames);

                if (leagues == null)
                {
                    return NotFound(new { Message = _localizer["LackOfLeaguesErrorMessage"].Value });
                }

                return Ok(leagues);
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
        public async Task<ActionResult<LeagueDTO>> GetLeagueByIdAsync(int id, [FromQuery] bool includeTeams = false, [FromQuery] bool includeGames = false)
        {
            try
            {
                var league = await _leagueService.GetLeagueByIdAsync(id, includeTeams, includeGames);

                if (league == null)
                {
                    return NotFound(new { Message = _localizer["LeagueNotFoundErrorMessage"].Value });
                }

                return Ok(league);
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

        [HttpGet("{leagueId:int}/teams")]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetLeagueTeamsAsync(int leagueId)
        {
            try
            {
                var league = await _leagueService.GetLeagueByIdAsync(leagueId, false, false);

                if (league == null)
                {
                    return NotFound(new { Message = _localizer["LeagueNotFoundErrorMessage"].Value });
                }

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
    }
}
