using Betto.Model.DTO;
using Betto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Betto.Resources.Shared;
using Microsoft.Extensions.Localization;

namespace Betto.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public TeamsController(ITeamService teamService, IStringLocalizer<ErrorMessages> localizer)
        {
            this._teamService = teamService;
            this._localizer = localizer;
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<TeamDTO>> GetTeamByIdAsync(int id, bool includeVenue = false)
        {
            try
            {
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
