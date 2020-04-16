﻿using Betto.Model.ViewModels;
using Betto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Models;
using Betto.Services.GameService;

namespace Betto.Api.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class LeaguesController : ControllerBase
    {
        private readonly ILeagueService _leagueService;
        private readonly ITeamService _teamService;
        private readonly IGameService _gameService;

        public LeaguesController(ILeagueService leagueService, 
            ITeamService teamService,
            IGameService gameService)
        {
            _leagueService = leagueService;
            _teamService = teamService;
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeagueViewModel>>> GetLeaguesAsync()
        {
            try
            {
                var response = await _leagueService.GetLeaguesAsync(false, false);

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
        
        [HttpGet("{leagueId:int}/games")]
        public async Task<ActionResult<IEnumerable<GameViewModel>>> GetLeagueGamesAsync([FromRoute] int leagueId)
        {
            try
            {
                var response = await _gameService.GetLeagueGamesAsync(leagueId);

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
