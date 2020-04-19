using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Models;
using Betto.Model.ViewModels;
using Betto.Model.WriteModels;
using Betto.Services;
using Betto.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Betto.Api.Controllers.UsersController
{
    [ApiController, Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService,
            ITicketService ticketService,
            IPaymentService paymentService,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _ticketService = ticketService;
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<WebTokenViewModel>> AuthenticateUserAsync([FromBody] LoginWriteModel loginData)
        {
            try
            {
                var response = await _userService.AuthenticateUserAsync(loginData);

                return response.StatusCode == StatusCodes.Status200OK
                    ? Ok(response.Result)
                    : StatusCode(response.StatusCode, response.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError(e, string.Empty);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ErrorViewModel.Factory.NewErrorFromException(e));
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserViewModel>> SignUpAsync([FromBody] RegistrationWriteModel signUpData)
        {
            try
            {
                var response = await _userService.SignUpAsync(signUpData);

                return response.StatusCode == StatusCodes.Status200OK
                    ? Ok(response.Result)
                    : StatusCode(response.StatusCode, response.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError(e, string.Empty);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ErrorViewModel.Factory.NewErrorFromException(e));
            }
        }

        [HttpGet("{userId:int}/tickets"), Authorize]
        public async Task<ActionResult<ICollection<TicketViewModel>>> GetUserTicketsAsync([FromRoute] int userId)
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
                _logger.LogError(e, string.Empty);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ErrorViewModel.Factory.NewErrorFromException(e));
            }
        }

        [HttpGet("{userId:int}/payments"), Authorize]
        public async Task<ActionResult<ICollection<PaymentViewModel>>> GetUserPaymentsAsync([FromRoute] int userId)
        {
            try
            {
                var response = await _paymentService.GetUserPaymentsAsync(userId);

                return response.StatusCode == StatusCodes.Status200OK
                    ? Ok(response.Result)
                    : StatusCode(response.StatusCode, response.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError(e, string.Empty);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ErrorViewModel.Factory.NewErrorFromException(e));
            }
        }

        [HttpGet("ranking")]
        public async Task<ActionResult<ICollection<PaymentViewModel>>> GetUsersRankingAsync()
        {
            try
            {
                var response = await _userService.GetUsersRankingAsync();

                return response.StatusCode == StatusCodes.Status200OK
                    ? Ok(response.Result)
                    : StatusCode(response.StatusCode, response.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError(e, string.Empty);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ErrorViewModel.Factory.NewErrorFromException(e));
            }
        }
    }
}
