using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.ViewModels;
using Betto.Model.WriteModels;
using Betto.Resources.Shared;
using Betto.Services;
using Betto.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Betto.Api.Controllers.UsersController
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IStringLocalizer<ErrorMessages> _localizer;
        private readonly ITicketService _ticketService;

        public UsersController(IUserService userService, 
            IStringLocalizer<ErrorMessages> localizer,
            ITicketService ticketService)
        {
            _userService = userService;
            _localizer = localizer;
            _ticketService = ticketService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<WebTokenViewModel>> AuthenticateUserAsync([FromBody] LoginWriteModel loginData)
        {
            try
            {
                var doesUserExist = await _userService.CheckIsUsernameAlreadyTakenAsync(loginData.Username);

                if (!doesUserExist)
                {
                    return NotFound(new { Message = _localizer["UserNotFoundErrorMessage", loginData.Username].Value });
                }

                var token = await _userService.AuthenticateUserAsync(loginData);

                if (token == null)
                {
                    return Unauthorized(new { Message = _localizer["IncorrectPasswordErrorMessage"].Value });
                }

                return Ok(token);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = e.InnerException != null
                            ? $"{e.Message} {e.InnerException.Message}"
                            : e.Message
                    });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserViewModel>> SignUpAsync([FromBody] SignUpWriteModel signUpData)
        {
            try
            {
                var doesUserExist = await _userService.CheckIsUsernameAlreadyTakenAsync(signUpData.Username);

                if (doesUserExist)
                {
                    return BadRequest(new { Message = _localizer["UsernameAlreadyTakenErrorMessage", signUpData.Username].Value });
                }

                doesUserExist = await _userService.CheckIsMailAlreadyTakenAsync(signUpData.MailAddress);

                if (doesUserExist)
                {
                    return BadRequest(new { Message = _localizer["MailAddressAlreadyTakenErrorMessage", signUpData.MailAddress].Value });
                }

                var user = await _userService.SignUpAsync(signUpData);

                if (user == null)
                {
                    return BadRequest(new { Message = _localizer["UserCreationErrorMessage"].Value });
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = e.InnerException != null
                            ? $"{e.Message} {e.InnerException.Message}"
                            : e.Message
                    });
            }
        }

        [HttpGet("{userId:int}/tickets"), Authorize]
        public async Task<ActionResult<ICollection<TicketViewModel>>> GetUserTicketsAsync(int userId)
        {
            try
            {
                var response = await _ticketService.GetUserTicketsAsync(userId);

                return response.StatusCode == StatusCodes.Status200OK
                    ? Ok(response.Result)
                    : StatusCode(response.StatusCode, response.Errors);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new
                    {
                        Message = e.InnerException != null
                            ? $"{e.Message} {e.InnerException.Message}"
                            : e.Message
                    });
            }
        }
    }
}
