using System;
using System.Threading.Tasks;
using Betto.Model.Models;
using Betto.Model.ViewModels;
using Betto.Model.WriteModels;
using Betto.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Betto.Api.Controllers.TicketsController
{
    [ApiController, Route("api/[controller]"), Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <summary>
        /// If named with async suffix CreatedAtAction method located in CreateTicketAsync fails. Error documented here => https://github.com/dotnet/aspnetcore/issues/15316
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        [HttpGet("{ticketId:int}")]
        public async Task<ActionResult<TicketViewModel>> GetTicketById([FromRoute] int ticketId)
        {
            try
            {
                var response = await _ticketService.GetTicketByIdAsync(ticketId);

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

        [HttpPost]
        public async Task<ActionResult<TicketViewModel>> CreateTicketAsync([FromBody] TicketWriteModel ticket)
        {
            try
            {
                var response = await _ticketService.AddTicketAsync(ticket);

                return response.StatusCode == StatusCodes.Status201Created
                    ? CreatedAtAction(nameof(GetTicketById), new {ticketId = response.Result.TicketId}, response.Result)
                    : StatusCode(response.StatusCode, response.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ErrorViewModel.Factory.NewErrorFromException(ex));
            }
        }

        [HttpPost("{ticketId:int}/reveal")]
        public async Task<ActionResult<TicketViewModel>> RevealTicketAsync(int ticketId)
        {
            try
            {
                var response = await _ticketService.RevealTicketAsync(ticketId);

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
