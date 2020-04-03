using System;
using System.Threading.Tasks;
using Betto.Model.DTO;
using Betto.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Betto.Api.Controllers.UsersController
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<WebTokenDTO>> AuthenticateUserAsync([FromBody] LoginDTO loginData)
        {
            try
            {
                var doesUserExist = await _userService.CheckIsUsernameAlreadyTakenAsync(loginData.Username);

                if (!doesUserExist)
                {
                    return NotFound($"User {loginData.Username} does not exist");
                }

                var token = await _userService.AuthenticateUserAsync(loginData);

                if (token == null)
                {
                    return Unauthorized("Incorrect password");
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
        public async Task<ActionResult<UserDTO>> SignUpAsync(SignUpDTO signUpData)
        {
            try
            {
                var doesUserExist = await _userService.CheckIsUsernameAlreadyTakenAsync(signUpData.Username);

                if (doesUserExist)
                {
                    return BadRequest($"Username {signUpData.Username} already taken");
                }

                doesUserExist = await _userService.CheckIsMailAlreadyTakenAsync(signUpData.MailAddress);

                if (doesUserExist)
                {
                    return BadRequest($"Mail {signUpData.MailAddress} already taken");
                }

                var user = await _userService.SignUpAsync(signUpData);

                if (user == null)
                {
                    return BadRequest("Could not create user");
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
