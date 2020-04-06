using Betto.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Betto.Api.Text;
using Microsoft.Extensions.Localization;

namespace Betto.Api.Controllers
{
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

        [HttpOptions("leagues")]
        public async Task<IActionResult> ImportDataAsync()
        {
            try
            {
                await _importService.ImportExternalDataAsync();

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
