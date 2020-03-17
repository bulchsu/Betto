using Betto.Services.Services.ImportService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Betto.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly IImportService _importService;

        public ImportController(IImportService importService)
        {
            this._importService = importService;
        }

        [HttpOptions("leagues")]
        public async Task<IActionResult> ImportDataAsync()
        {
            try
            {
                await _importService.ImportExternalDataAsync();

                return Ok(new { Message = "Successfully imported leagues from external server" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.InnerException != null ? ex.Message + ex.InnerException.Message : ex.Message });
            }
        }
    }
}
