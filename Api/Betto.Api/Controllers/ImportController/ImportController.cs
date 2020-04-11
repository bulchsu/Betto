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
    public class ImportController : ControllerBase
    {
        private readonly IImportService _importService;
        private readonly IStringLocalizer<InformationMessages> _localizer;

        public ImportController(IImportService importService, IStringLocalizer<InformationMessages> localizer)
        {
            this._importService = importService;
            this._localizer = localizer;
        }

        [HttpOptions("initial")]
        public async Task<IActionResult> ImportInitialDataAsync()
        {
            try
            {
                await _importService.ImportInitialDataAsync();

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

        [HttpOptions("leagues/next/{amount:int}")]
        public async Task<IActionResult> ImportAdditionalLeaguesAsync(int amount)
        {
            try
            {
                await _importService.ImportNextLeaguesAsync(amount);

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
