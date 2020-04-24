using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.DataAccessLayer.Repositories;
using Betto.DataAccessLayer.Repositories.RatesRepository;
using Betto.Helpers.Extensions;
using Betto.Model.ViewModels;
using Betto.Model.Entities;
using Betto.Model.Models;
using Betto.Model.WriteModels;
using Betto.Resources.Shared;
using Betto.Services.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Betto.Services.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IRatesRepository _ratesRepository;
        private readonly ITicketValidator _ticketValidator;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public TicketService(ITicketRepository ticketRepository,
            IUserRepository userRepository,
            IGameRepository gameRepository,
            IRatesRepository ratesRepository,
            ITicketValidator ticketValidator,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _ratesRepository = ratesRepository;
            _ticketValidator = ticketValidator;
            _localizer = localizer;
        }

        public async Task<RequestResponseModel<TicketViewModel>> GetTicketByIdAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);

            if (ticket == null)
            {
                return new RequestResponseModel<TicketViewModel>(StatusCodes.Status404NotFound,
                    new List<ErrorViewModel>
                    { 
                        ErrorViewModel.Factory.NewErrorFromMessage(_localizer["TicketNotFoundErrorMessage", 
                            ticketId]
                            .Value)
                    },
                    null);
            }

            var result = await _ticketValidator.PrepareResponseTicketAsync(ticket);

            return new RequestResponseModel<TicketViewModel>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(), 
                result);
        }

        public async Task<RequestResponseModel<TicketViewModel>> AddTicketAsync(TicketWriteModel ticket)
        {
            var validationResults = await _ticketValidator.ValidateTicketBeforeSavingAsync(ticket);

            if (validationResults.Any())
            {
                return new RequestResponseModel<TicketViewModel>(StatusCodes.Status400BadRequest,
                    validationResults,
                    null);
            }

            var createdTicket = await SaveTicketAsync(ticket);

            if (createdTicket == null)
            {
                return new RequestResponseModel<TicketViewModel>(StatusCodes.Status400BadRequest,
                    new List<ErrorViewModel> 
                    { 
                        ErrorViewModel.Factory.NewErrorFromMessage(_localizer["TicketNotCreatedErrorMessage"]
                            .Value)
                    },
                    null);
            }

            var result = await _ticketValidator.PrepareResponseTicketAsync(createdTicket);

            return new RequestResponseModel<TicketViewModel>(StatusCodes.Status201Created, 
                Enumerable.Empty<ErrorViewModel>(), 
                result);
        }

        public async Task<RequestResponseModel<TicketViewModel>> RevealTicketAsync(int ticketId)
        {
            var validationResults = await _ticketValidator.ValidateTicketBeforeRevealingAsync(ticketId);

            if (validationResults.Any())
            {
                return new RequestResponseModel<TicketViewModel>(StatusCodes.Status400BadRequest,
                    validationResults,
                    null);
            }

            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
            await DetermineTicketResultAsync(ticket);

            var result = await _ticketValidator.PrepareResponseTicketAsync(ticket);

            return new RequestResponseModel<TicketViewModel>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(),
                result);
        }

        private async Task<TicketEntity> SaveTicketAsync(TicketWriteModel ticket)
        {
            var ticketEntity = (TicketEntity) ticket;

            await FindEventsRatesAsync(ticketEntity);
            await FindEventResultsAsync(ticketEntity.Events);

            await ReduceUserAccountBalanceAsync(ticket.UserId, ticket.Stake);
            var createdTicket = await _ticketRepository.AddTicketAsync(ticketEntity);
            
            await _ticketRepository.SaveChangesAsync();

            return createdTicket;
        }

        private async Task DetermineTicketResultAsync(TicketEntity ticket)
        {
            var isWon = _ticketValidator.CheckIsTicketWon(ticket);
            SetTicketResults(isWon, ticket);

            if (isWon)
            {
                var price = ticket.Stake * ticket.TotalConfirmedRate;
                await ModifyUserAccountBalanceAsync(ticket.UserId, price);
            }

            await _ticketRepository.SaveChangesAsync();
        }

        private async Task ModifyUserAccountBalanceAsync(int userId, double amountToAdd)
        {
            var user = await _userRepository.GetUserByIdAsync(userId, false, false);
            user.AccountBalance += Math.Round(amountToAdd, 2);
        }

        private static void SetTicketResults(bool isWon, TicketEntity ticket)
        {
            ticket.Status = isWon ? StatusEnum.Won : StatusEnum.Lost;
            ticket.RevealDateTime = DateTime.Now;
        }

        private async Task FindEventResultsAsync(ICollection<EventEntity> events)
        {
            var gameIds = events.Select(g => g.GameId);
            var games = await _gameRepository.GetGamesByBunchOfIdsAsync(gameIds);

            foreach (var eventEntity in events)
            {
                var relatedGame = games.SingleOrDefault(g => g.GameId == eventEntity.GameId);
                var gameResult = CheckGameResult(relatedGame.GoalsHomeTeam, relatedGame.GoalsAwayTeam);

                eventEntity.EventStatus = gameResult == eventEntity.BetType ? StatusEnum.Won : StatusEnum.Lost;
            }
        }

        private async Task ReduceUserAccountBalanceAsync(int userId, double charge)
        {
            var user = await _userRepository.GetUserByIdAsync(userId, false, false);
            user.AccountBalance -= charge;
        }

        private static BetTypeEnum CheckGameResult(int homeTeamGoals, int awayTeamGoals)
        {
            var result = BetTypeEnum.HomeTeamWin;

            if (homeTeamGoals == awayTeamGoals)
            {
                result = BetTypeEnum.Tie;
            }
            else if (awayTeamGoals > homeTeamGoals)
            {
                result = BetTypeEnum.AwayTeamWin;
            }

            return result;
        }

        private async Task FindEventsRatesAsync(TicketEntity ticket)
        {
            foreach (var eventEntity in ticket.Events.GetEmptyIfNull())
            {
                var rates = await _ratesRepository.GetGameRatesAsync(eventEntity.GameId);
                eventEntity.ConfirmedRate = GetCorrectRate(eventEntity.BetType, rates);
            }

            ticket.TotalConfirmedRate = CalculateTotalConfirmedRate(ticket.Events);
        }

        private float GetCorrectRate(BetTypeEnum type, BetRatesEntity rates) =>
            type switch
            {
                BetTypeEnum.HomeTeamWin => rates.HomeTeamWinRate,
                BetTypeEnum.AwayTeamWin => rates.AwayTeamWinRate,
                BetTypeEnum.Tie => rates.TieRate,
                _ => throw new ArgumentException(_localizer["IncorrectBetTypeErrorMessage", type].Value, nameof(type))
            };

        private static float CalculateTotalConfirmedRate(IEnumerable<EventEntity> events) =>
            (float) Math.Round(events.Select(e => e.ConfirmedRate).Product(), 2);
    }
}
