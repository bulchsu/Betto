using Betto.Model.DTO;
using Betto.Services.Services.LeagueService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Betto.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaguesController : ControllerBase
    {
        private readonly ILeagueService _leagueService;

        public LeaguesController(ILeagueService leagueService)
        {
            _leagueService = leagueService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeagueDTO>>> GetLeaguesAsync()
        {
            try
            {
                var leagues = await _leagueService.GetLeaguesAsync();

                if (leagues == null)
                    return NotFound(new { Message = "There are no leagues in the database" });

                return Ok(leagues);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.InnerException != null ? $"{ex.Message} {ex.InnerException.Message}" : ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LeagueDTO>> GetLeagueByIdAsync(int id)
        {
            try
            {
                var league = await _leagueService.GetLeagueByIdAsync(id);

                if (league == null)
                    return NotFound(new { Message = "League not found" });

                return Ok(league);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.InnerException != null ? $"{ex.Message} {ex.InnerException.Message}" : ex.Message });
            }
        }
    }
}
