using Betto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Betto.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Betto.Api.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize(Roles = Role.Admin)]
    public class OptionsController : ControllerBase
    {
        private readonly IOptionsService _optionsService;
        private readonly ILogger<OptionsController> _logger;

        public OptionsController(IOptionsService optionsService,
            ILogger<OptionsController> logger)
        {
            _optionsService = optionsService;
            _logger = logger;
        }

        [HttpOptions("initialize")]
        public async Task<IActionResult> ImportInitialDataAsync()
        {
            try
            {
                var response = await _optionsService.ImportInitialDataAsync();

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

        [HttpOptions("add/next/{amount:int}")]
        public async Task<IActionResult> ImportAdditionalLeaguesAsync(int amount)
        {
            try
            {
                var response = await _optionsService.ImportNextLeaguesAsync(amount);

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
