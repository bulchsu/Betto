using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.DataAccessLayer.Repositories;
using Betto.Helpers;
using Betto.Model.ViewModels;
using Betto.Model.Entities;
using Betto.Model.Models;
using Betto.Model.WriteModels;
using Betto.Resources.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Linq;
using Betto.Helpers.Extensions;
using Betto.Services.Validators;

namespace Betto.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUserValidator _userValidator;
        private readonly ITicketValidator _ticketValidator;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public UserService(IUserRepository userRepository,
            ITicketRepository ticketRepository,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IUserValidator userValidator,
            ITicketValidator ticketValidator,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _userValidator = userValidator;
            _ticketValidator = ticketValidator;
            _localizer = localizer;
        }

        public async Task<RequestResponseModel<AuthenticationViewModel>> AuthenticateUserAsync(LoginWriteModel loginModel)
        {
            var errors = await _userValidator.CheckLoginCredentials(loginModel);

            if (errors.Any())
            {
                return new RequestResponseModel<AuthenticationViewModel>(StatusCodes.Status400BadRequest, 
                    errors, 
                    null);
            }

            var authenticationToken = _tokenGenerator.GenerateToken(loginModel.Username);
            var user = await _userRepository.GetUserByUsernameAsync(loginModel.Username);

            return new RequestResponseModel<AuthenticationViewModel>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(),
                new AuthenticationViewModel
                {
                    AuthenticationToken = authenticationToken,
                    Username = loginModel.Username,
                    UserId = user.UserId
                });
        }

        public async Task<RequestResponseModel<UserViewModel>> SignUpAsync(RegistrationWriteModel signUpData)
        {
            var errors = await _userValidator.CheckSignUpDataBeforeRegisteringAsync(signUpData);

            if (errors.Any())
            {
                return new RequestResponseModel<UserViewModel>(StatusCodes.Status400BadRequest, 
                    errors, 
                    null);
            }

            var passwordHash = _passwordHasher.EncodePassword(signUpData.Password);

            var registeredUser = await _userRepository.SignUpAsync(new UserEntity
            {
                MailAddress = signUpData.MailAddress,
                Username = signUpData.Username,
                PasswordHash = passwordHash
            });

            if (registeredUser == null)
            {
                return new RequestResponseModel<UserViewModel>(StatusCodes.Status400BadRequest,
                    new List<ErrorViewModel>
                    {
                        ErrorViewModel.Factory.NewErrorFromMessage(_localizer["UserCreationErrorMessage"]
                                .Value)
                    },
                    null);
            }

            await _userRepository.SaveChangesAsync();
            var result = (UserViewModel) registeredUser;

            return new RequestResponseModel<UserViewModel>(StatusCodes.Status200OK, 
                Enumerable.Empty<ErrorViewModel>(), 
                result);
        }

        public async Task<RequestResponseModel<ICollection<UserRankingPositionViewModel>>> GetUsersRankingAsync()
        {
            var rankingPositions = await GetUsersRanking();

            return new RequestResponseModel<ICollection<UserRankingPositionViewModel>>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(),
                rankingPositions);
        }

        public async Task<RequestResponseModel<UserViewModel>> GetUserByIdAsync(int userId, 
            bool includePayments, bool includeTickets)
        {
            var user = await _userRepository.GetUserByIdAsync(userId, includePayments, includeTickets);

            if (user == null)
            {
                return new RequestResponseModel<UserViewModel>(StatusCodes.Status404NotFound,
                    new List<ErrorViewModel>
                    {
                        ErrorViewModel.Factory.NewErrorFromMessage(_localizer["UserNotFoundErrorMessage",
                                userId]
                            .Value)
                    },
                    null);
            }

            var jointTask = user.Tickets
                .GetEmptyIfNull()
                .Select(_ticketValidator.PrepareResponseTicketAsync);
            
            var userViewModel = (UserViewModel) user;
            userViewModel.Tickets = (await Task.WhenAll(jointTask))
                .ToList();

            return new RequestResponseModel<UserViewModel>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(),
                userViewModel);
        }

        private async Task<ICollection<UserRankingPositionViewModel>> GetUsersRanking()
        {
            var users = await _userRepository.GetUsersAsync();
            var tickets = await _ticketRepository.GetTicketsAsync();

            var rankingPositions = (from user in users
                    let userTickets = tickets.Where(t => t.UserId == user.UserId)
                    let wonTickets = userTickets.Where(t => t.Status == StatusEnum.Won)
                    let wonAmount = wonTickets
                        .Sum(t => t.Stake * (t.TotalConfirmedRate - 1))
                    let lostAmount = userTickets
                        .Where(t => t.Status == StatusEnum.Lost)
                        .Sum(t => t.Stake)
                    select new UserRankingPositionViewModel
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        HighestPrice = wonTickets.Any() 
                            ? Math.Round(wonTickets.Max(t => t.Stake * t.TotalConfirmedRate), 2)
                            : 0,
                        TotalWonAmount = Math.Round(wonAmount - lostAmount, 2)
                    }).OrderByDescending(t => t.TotalWonAmount)
                .ThenByDescending(t => t.HighestPrice)
                .ToList()
                .GetEmptyIfNull()
                .FillPositions();

            return rankingPositions;
        }
    }
}
