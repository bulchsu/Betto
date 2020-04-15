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
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public TicketService(ITicketRepository ticketRepository,
            IUserRepository userRepository,
            IGameRepository gameRepository,
            IRatesRepository ratesRepository,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _ratesRepository = ratesRepository;
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

            var result = PrepareResponseTicket(ticket);

            return new RequestResponseModel<TicketViewModel>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(), 
                result);
        }

        public async Task<RequestResponseModel<IEnumerable<TicketViewModel>>> GetUserTicketsAsync(int userId)
        {
            var doesUserExist = await CheckDoesUserExistAsync(userId);

            if (!doesUserExist)
            {
                return new RequestResponseModel<IEnumerable<TicketViewModel>>(StatusCodes.Status404NotFound,
                    new List<ErrorViewModel>
                    { 
                        ErrorViewModel.Factory.NewErrorFromMessage(_localizer["UserNotFoundErrorMessage", 
                            userId]
                            .Value)
                    }, 
                    null);
            }

            var userTickets = (await _ticketRepository.GetUserTicketsAsync(userId))
                .ToList()
                .GetEmptyIfNull();

            var results = new List<TicketViewModel>(userTickets.Select(PrepareResponseTicket))
                .GetEmptyIfNull();

            return new RequestResponseModel<IEnumerable<TicketViewModel>>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(),
                    results);
        }

        public async Task<RequestResponseModel<TicketViewModel>> AddTicketAsync(TicketWriteModel ticket)
        {
            var validationResults = await ValidateTicketBeforeSavingAsync(ticket);

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

            var result = PrepareResponseTicket(createdTicket);

            return new RequestResponseModel<TicketViewModel>(StatusCodes.Status201Created, 
                Enumerable.Empty<ErrorViewModel>(), 
                result);
        }

        public async Task<RequestResponseModel<TicketViewModel>> RevealTicketAsync(int ticketId)
        {
            var validationResults = await ValidateTicketBeforeRevealingAsync(ticketId);

            if (validationResults.Any())
            {
                return new RequestResponseModel<TicketViewModel>(StatusCodes.Status400BadRequest,
                    validationResults,
                    null);
            }

            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
            await DetermineTicketResultAsync(ticket);

            var result = PrepareResponseTicket(ticket);

            return new RequestResponseModel<TicketViewModel>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(),
                result);
        }

        private async Task<ICollection<ErrorViewModel>> ValidateTicketBeforeSavingAsync(TicketWriteModel ticket)
        {
            var errors = new List<ErrorViewModel>();

            CheckDoesTicketContainEvents(ticket, errors);
            var doesExist = await CheckDoesUserExistAsync(ticket.UserId);

            if (!doesExist)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["UserNotFoundErrorMessage",
                    ticket.UserId]
                    .Value));

                return errors;
            }

            await CheckIsTicketAffordableByUserAsync(ticket, errors);
            await CheckHasUserPlayedAnyOfGamesBeforeAsync(ticket, errors);

            return errors;
        }

        private async Task<ICollection<ErrorViewModel>> ValidateTicketBeforeRevealingAsync(int ticketId)
        {
            var errors = new List<ErrorViewModel>();
            var doesExist = await CheckDoesTicketExistAsync(ticketId);

            if (!doesExist)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["TicketNotFoundErrorMessage", 
                    ticketId]
                    .Value));

                return errors;
            }

            await CheckIsTicketAlreadyRevealedAsync(ticketId, errors);

            return errors;
        }

        private void CheckDoesTicketContainEvents(TicketWriteModel ticket, ICollection<ErrorViewModel> errors)
        {
            if (!ticket.Events.GetEmptyIfNull().Any())
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["LackOfEventsErrorMessage"]
                    .Value));
            }
        }

        private async Task<bool> CheckDoesUserExistAsync(int userId) =>
            await _userRepository.GetUserByIdAsync(userId) != null;

        private async Task CheckIsTicketAffordableByUserAsync(TicketWriteModel ticketModel,
            ICollection<ErrorViewModel> errors)
        {
            var user = await _userRepository.GetUserByIdAsync(ticketModel.UserId);

            if (ticketModel.Stake > user.AccountBalance)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["TicketNotAffordableErrorMessage", 
                    ticketModel.Stake, 
                    user.AccountBalance]
                    .Value));
            }
        }

        private async Task CheckHasUserPlayedAnyOfGamesBeforeAsync(TicketWriteModel ticket, ICollection<ErrorViewModel> errors)
        {
            var gameIds = ticket.Events.Select(e => e.GameId);
            var userTickets = await _ticketRepository.GetUserTicketsAsync(ticket.UserId);
            var playedGames = userTickets.SelectMany(t => t.Events);
            var isTicketInvalid = playedGames.Any(g => gameIds.Contains(g.GameId));

            if (isTicketInvalid)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["EventsRepeatedByUserErrorMessage"]
                    .Value));
            }
        }

        private async Task<bool> CheckDoesTicketExistAsync(int ticketId) =>
            await _ticketRepository.GetTicketByIdAsync(ticketId) != null;

        private async Task CheckIsTicketAlreadyRevealedAsync(int ticketId, ICollection<ErrorViewModel> errors)
        {
            var ticket = (TicketViewModel) await _ticketRepository.GetTicketByIdAsync(ticketId);
            var isAlreadyRevealed = IsTicketRevealed(ticket);

            if (isAlreadyRevealed)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["TicketAlreadyRevealedErrorMessage",
                        ticketId]
                    .Value));
            }
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
            var isWon = VerifyTicketEvents(ticket.Events);
            SetTicketResults(isWon, ticket);

            await _ticketRepository.SaveChangesAsync();
        }

        private static TicketViewModel PrepareResponseTicket(TicketEntity ticket)
        {
            var result = (TicketViewModel) ticket;
            FilterTicketEventsResults(result);

            return result;
        }
        private static void FilterTicketEventsResults(TicketViewModel ticket)
        {
            var isRevealed = IsTicketRevealed(ticket);

            if (!isRevealed)
            {
                HideEventResult(ticket.Events);
            }
        }

        private static bool IsTicketRevealed(TicketViewModel ticket) =>
            ticket.Status != StatusEnum.Pending && ticket.RevealDateTime.HasValue;


        private static void HideEventResult(IEnumerable<TicketEventViewModel> events)
        {
            foreach (var eventViewModel in events)
            {
                eventViewModel.EventStatus = StatusEnum.Pending;
            }
        }

        private static bool VerifyTicketEvents(IEnumerable<EventEntity> events) =>
            events.All(e => e.EventStatus == StatusEnum.Won);

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
            var user = await _userRepository.GetUserByIdAsync(userId);
            user.AccountBalance -= charge;
        }

        private static BetType CheckGameResult(int homeTeamGoals, int awayTeamGoals)
        {
            var result = BetType.HomeTeamWin;

            if (homeTeamGoals == awayTeamGoals)
            {
                result = BetType.Tie;
            }
            else if (awayTeamGoals > homeTeamGoals)
            {
                result = BetType.AwayTeamWin;
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

        private float GetCorrectRate(BetType type, BetRatesEntity rates) =>
            type switch
            {
                BetType.HomeTeamWin => rates.HomeTeamWinRate,
                BetType.AwayTeamWin => rates.AwayTeamWinRate,
                BetType.Tie => rates.TieRate,
                _ => throw new ArgumentException(_localizer["IncorrectBetTypeErrorMessage", type].Value, nameof(type))
            };

        private static float CalculateTotalConfirmedRate(IEnumerable<EventEntity> events) =>
            (float) Math.Round(events.Select(e => e.ConfirmedRate).Product(), 2);
    }
}
