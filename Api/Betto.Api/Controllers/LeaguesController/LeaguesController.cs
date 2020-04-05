using Betto.Model.DTO;
using Betto.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Api.Text;
using Microsoft.Extensions.Localization;

namespace Betto.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LeaguesController : ControllerBase
    {
        private readonly ILeagueService _leagueService;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public LeaguesController(ILeagueService leagueService, IStringLocalizer<ErrorMessages> localizer)
        {
            this._leagueService = leagueService;
            this._localizer = localizer;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeagueDTO>>> GetLeaguesAsync()
        {
            try
            {
                var leagues = await _leagueService.GetLeaguesAsync();

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
        public async Task<ActionResult<LeagueDTO>> GetLeagueByIdAsync(int id)
        {
            try
            {
                var league = await _leagueService.GetLeagueByIdAsync(id);

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
    }
}
