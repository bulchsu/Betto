using Betto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Betto.Model.Models;
using Microsoft.AspNetCore.Authorization;

namespace Betto.Api.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize]
    public class OptionsController : ControllerBase
    {
        private readonly IOptionsService _optionsService;

        public OptionsController(IOptionsService optionsService)
        {
            _optionsService = optionsService;
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
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ErrorViewModel.Factory.NewErrorFromException(ex));
            }
        }
    }
}
