using System;
using System.Threading.Tasks;
using Betto.Model.Models;
using Betto.Model.ViewModels;
using Betto.Services.GameService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Betto.Api.Controllers.GamesController
{
    [ApiController, Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GamesController> _logger;

        public GamesController(IGameService gameService,
            ILogger<GamesController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet("{gameId:int}")]
        public async Task<ActionResult<GameViewModel>> GetGameByIdAsync([FromRoute] int gameId, 
            [FromQuery] bool includeRates = false)
        {
            try
            {
                var response = await _gameService.GetGameByIdAsync(gameId, includeRates);

                return response.StatusCode == StatusCodes.Status200OK
                    ? Ok(response.Result)
                    : StatusCode(response.StatusCode, response.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Empty);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ErrorViewModel.Factory.NewErrorFromException(ex));
            }
        }
    }
}