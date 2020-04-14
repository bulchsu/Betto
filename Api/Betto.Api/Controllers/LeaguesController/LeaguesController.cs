using Betto.Model.ViewModels;
using Betto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Models;

namespace Betto.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class LeaguesController : ControllerBase
    {
        private readonly ILeagueService _leagueService;
        private readonly ITeamService _teamService;

        public LeaguesController(ILeagueService leagueService, 
            ITeamService teamService)
        {
            _leagueService = leagueService;
            _teamService = teamService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeagueViewModel>>> GetLeaguesAsync([FromQuery] bool includeTeams = false, 
            [FromQuery] bool includeGames = false)
        {
            try
            {
                var response = await _leagueService.GetLeaguesAsync(includeTeams, includeGames);

                return response.StatusCode == StatusCodes.Status200OK
                    ? Ok(response.Result)
                    : StatusCode(response.StatusCode, response.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ErrorViewModel.Factory.NewErrorFromException(ex));
            }
        }

        [HttpGet("{leagueId:int}")]
        public async Task<ActionResult<LeagueViewModel>> GetLeagueByIdAsync([FromRoute] int leagueId, 
            [FromQuery] bool includeTeams = false, [FromQuery] bool includeGames = false)
        {
            try
            {
                var response = await _leagueService.GetLeagueByIdAsync(leagueId, includeTeams, includeGames);

                return response.StatusCode == StatusCodes.Status200OK
                    ? Ok(response.Result)
                    : StatusCode(response.StatusCode, response.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ErrorViewModel.Factory.NewErrorFromException(ex));
            }
        }

        [HttpGet("{leagueId:int}/teams")]
        public async Task<ActionResult<IEnumerable<TeamViewModel>>> GetLeagueTeamsAsync([FromRoute] int leagueId)
        {
            try
            {
                var response = await _teamService.GetLeagueTeamsAsync(leagueId);

                return response.StatusCode == StatusCodes.Status200OK
                    ? Ok(response.Result)
                    : StatusCode(response.StatusCode, response.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ErrorViewModel.Factory.NewErrorFromException(ex));
            }
        }
    }
}
