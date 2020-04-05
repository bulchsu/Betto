using System;
using System.Threading.Tasks;
using Betto.Api.Text;
using Betto.Model.DTO;
using Betto.Services.Services;
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

        public UsersController(IUserService userService, IStringLocalizer<ErrorMessages> localizer)
        {
            this._userService = userService;
            this._localizer = localizer;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<WebTokenDTO>> AuthenticateUserAsync([FromBody] LoginDTO loginData)
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
        public async Task<ActionResult<UserDTO>> SignUpAsync([FromBody] SignUpDTO signUpData)
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
    }
}
