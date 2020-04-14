using System;
using System.Linq;
using System.Threading.Tasks;
using Betto.Helpers.Extensions;
using Betto.Model.DTO;
using Betto.Resources.Shared;
using Betto.Services;
using Betto.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Betto.Api.Controllers.TicketsController
{
    [ApiController, Route("api/[controller]"), Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public TicketsController(ITicketService ticketService,
            IUserService userService,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _ticketService = ticketService;
            _userService = userService;
            _localizer = localizer;
        }

        /// <summary>
        /// If named with async suffix CreatedAtAction method located in CreateTicketAsync fails. Error documented here => https://github.com/dotnet/aspnetcore/issues/15316
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        [HttpGet("{ticketId:int}")]
        public async Task<ActionResult<TicketDTO>> GetTicketById([FromRoute] int ticketId)
        {
            try
            {
                var ticket = await _ticketService.GetTicketByIdAsync(ticketId);

                if (ticket == null)
                {
                    return NotFound(new { Message = _localizer["TicketNotFoundErrorMessage", ticketId].Value });
                }

                return Ok(ticket);
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

        [HttpPost]
        public async Task<ActionResult<TicketDTO>> CreateTicketAsync([FromBody] CreateTicketDTO ticket)
        {
            try
            {
                if (!ticket.Events.GetEmptyIfNull().Any())
                {
                    return BadRequest(new {Message = _localizer["LackOfEventsErrorMessage"].Value});
                }

                var user = await _userService.GetUserByIdAsync(ticket.UserId);

                if (user == null)
                {
                    return NotFound(new { Message = _localizer["UserNotFoundErrorMessage", ticket.UserId].Value });
                }

                var invalidTicket = await _ticketService.CheckHasUserPlayedAnyOfGamesBeforeAsync(ticket);

                if (invalidTicket)
                {
                    return BadRequest(new { Message = _localizer["EventsRepeatedByUserErrorMessage"].Value });
                }

                var createdTicket = await _ticketService.AddTicketAsync(ticket);

                if (createdTicket == null)
                {
                    return BadRequest(new { Message = _localizer["TicketNotCreatedErrorMessage"].Value });
                }
                
                return CreatedAtAction(nameof(GetTicketById), new {ticketId = createdTicket.TicketId}, createdTicket);
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

        [HttpPost("{ticketId:int}/reveal")]
        public async Task<ActionResult<TicketDTO>> RevealTicketAsync(int ticketId)
        {
            try
            {
                var ticket = await _ticketService.GetTicketByIdAsync(ticketId);

                if (ticket == null)
                {
                    return NotFound(new { Message = _localizer["TicketNotFoundErrorMessage", ticketId].Value });
                }

                var revealedTicket = await _ticketService.RevealTicketAsync(ticketId);

                if (revealedTicket == null)
                {
                    return BadRequest(new { Message = _localizer["RevealTicketErrorMessage", ticketId].Value });
                }

                return Ok(revealedTicket);
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
