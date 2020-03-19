using Betto.Model.DTO;
using Betto.Services.Services.LeagueService;
using Betto.Services.Services.TeamService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Api.Controllers
{
    [ApiController]
    [Route("api/leagues/{leagueId}/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ILeagueService _leagueService;
        private readonly ITeamService _teamService;

        public TeamsController(ILeagueService leagueService, ITeamService teamService)
        {
            this._leagueService = leagueService;
            this._teamService = teamService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetLeagueTeamsAsync(int leagueId)
        {
            try
            {
                var teams = await _teamService.GetLeagueTeamsAsync(leagueId);

                if (teams == null)
                    return NotFound(new { Message = "Teams not found" });

                return Ok(teams);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.InnerException != null ? $"{ex.Message} {ex.InnerException.Message}" : ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TeamDTO>> GetTeamByIdAsync(int leagueId, int id, bool includeVenues = false)
        {
            try
            {
                var league = await _leagueService.GetLeagueByIdAsync(leagueId);

                if (league == null)
                    return NotFound(new { Message = "League not found" });

                var team = await _teamService.GetTeamByIdAsync(id, includeVenues);

                if (team == null)
                    return NotFound(new { Message = "Team not found" });

                return Ok(team);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.InnerException != null ? $"{ex.Message} {ex.InnerException.Message}" : ex.Message });
            }
        }
    }
}
