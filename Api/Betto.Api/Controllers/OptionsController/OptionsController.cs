using Betto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Betto.Resources.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace Betto.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OptionsController : ControllerBase
    {
        private readonly IOptionsService _optionsService;
        private readonly IStringLocalizer<InformationMessages> _localizer;

        public OptionsController(IOptionsService optionsService, IStringLocalizer<InformationMessages> localizer)
        {
            _optionsService = optionsService;
            _localizer = localizer;
        }

        [HttpOptions("initialize")]
        public async Task<IActionResult> ImportInitialDataAsync()
        {
            try
            {
                await _optionsService.ImportInitialDataAsync();
                await _optionsService.SetBetRatesForAllLeaguesAsync();

                return Ok(new { Message = _localizer["SuccessfulImportMessage"].Value });
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

        [HttpOptions("add/next/{amount:int}")]
        public async Task<IActionResult> ImportAdditionalLeaguesAsync(int amount)
        {
            try
            {
                await _optionsService.ImportNextLeaguesAsync(amount);
                await _optionsService.SetBetRatesForAdditionalLeaguesAsync(amount);

                return Ok(new { Message = _localizer["SuccessfulImportMessage"].Value });
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
