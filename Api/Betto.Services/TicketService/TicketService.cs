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

        public async Task<RequestResponse<TicketViewModel>> GetTicketByIdAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);

            if (ticket == null)
            {
                return new RequestResponse<TicketViewModel>(StatusCodes.Status404NotFound,
                    new List<ErrorViewModel>{ new ErrorViewModel { Message = _localizer["TicketNotFoundErrorMessage", ticketId].Value }},
                    null);
            }

            var result = PrepareResponseTicket(ticket);

            return new RequestResponse<TicketViewModel>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(), 
                result);
        }

        public async Task<RequestResponse<IEnumerable<TicketViewModel>>> GetUserTicketsAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return new RequestResponse<IEnumerable<TicketViewModel>>(StatusCodes.Status404NotFound,
                    new List<ErrorViewModel>{ new ErrorViewModel { Message = _localizer["UserNotFoundErrorMessage", userId].Value }}, 
                    null);
            }

            var userTickets = (await _ticketRepository.GetUserTicketsAsync(userId)).ToList();
            var results = new List<TicketViewModel>(userTickets.Select(PrepareResponseTicket))
                .GetEmptyIfNull();

            return new RequestResponse<IEnumerable<TicketViewModel>>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(),
                    results);
        }

        public async Task<RequestResponse<TicketViewModel>> AddTicketAsync(TicketWriteModel ticket)
        {
            var validationResults = await ValidateTicketBeforeSavingAsync(ticket);

            if (validationResults.Any())
            {
                return new RequestResponse<TicketViewModel>(StatusCodes.Status400BadRequest,
                    validationResults,
                    null);
            }

            var createdTicket = await SaveTicketAsync(ticket);

            if (createdTicket == null)
            {
                return new RequestResponse<TicketViewModel>(StatusCodes.Status400BadRequest,
                    new List<ErrorViewModel> { new ErrorViewModel { Message = _localizer["TicketNotCreatedErrorMessage"].Value }},
                    null);
            }

            var result = PrepareResponseTicket(createdTicket);

            return new RequestResponse<TicketViewModel>(StatusCodes.Status201Created, 
                Enumerable.Empty<ErrorViewModel>(), 
                result);
        }

        public async Task<RequestResponse<TicketViewModel>> RevealTicketAsync(int ticketId)
        {
            var validationResults = await ValidateTicketBeforeRevealingAsync(ticketId);

            if (validationResults.Any())
            {
                return new RequestResponse<TicketViewModel>(StatusCodes.Status400BadRequest,
                    validationResults,
                    null);
            }

            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
            await DetermineTicketResultAsync(ticket);

            var result = PrepareResponseTicket(ticket);

            return new RequestResponse<TicketViewModel>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(),
                result);
        }

        private async Task<ICollection<ErrorViewModel>> ValidateTicketBeforeSavingAsync(TicketWriteModel ticket)
        {// TODO sprawdzic czy uzytkownika stac na kupon
            var errors = new List<ErrorViewModel>();

            CheckDoesTicketContainEvents(ticket, errors);
            var doesExist = await CheckDoesOwnerOfTheTicketExistAsync(ticket.UserId, errors);

            if (doesExist)
            {
                await CheckHasUserPlayedAnyOfGamesBeforeAsync(ticket, errors);
            }

            return errors;
        }

        private async Task<ICollection<ErrorViewModel>> ValidateTicketBeforeRevealingAsync(int ticketId)
        {
            var errors = new List<ErrorViewModel>();
            var doesExist = await CheckDoesTicketExistAsync(ticketId, errors);

            if (doesExist)
            {
                await CheckIsTicketAlreadyRevealedAsync(ticketId, errors);
            }

            return errors;
        }

        private void CheckDoesTicketContainEvents(TicketWriteModel ticket, ICollection<ErrorViewModel> errors)
        {
            if (!ticket.Events.GetEmptyIfNull().Any())
            {
                errors.Add(new ErrorViewModel { Message = _localizer["LackOfEventsErrorMessage"].Value });
            }
        }

        private async Task<bool> CheckDoesOwnerOfTheTicketExistAsync(int userId, ICollection<ErrorViewModel> errors)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                errors.Add(new ErrorViewModel { Message = _localizer["UserNotFoundErrorMessage", userId].Value });
            }

            return user != null;
        }

        private async Task CheckHasUserPlayedAnyOfGamesBeforeAsync(TicketWriteModel ticket, ICollection<ErrorViewModel> errors)
        {
            var gameIds = ticket.Events.Select(e => e.GameId);
            var userTickets = await _ticketRepository.GetUserTicketsAsync(ticket.UserId);
            var playedGames = userTickets.SelectMany(t => t.Events);
            var isTicketInvalid = playedGames.Any(g => gameIds.Contains(g.GameId));

            if (isTicketInvalid)
            {
                errors.Add(new ErrorViewModel { Message = _localizer["EventsRepeatedByUserErrorMessage"].Value });
            }
        }

        private async Task<bool> CheckDoesTicketExistAsync(int ticketId, ICollection<ErrorViewModel> errors)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);

            if (ticket == null)
            {
                errors.Add(new ErrorViewModel { Message = _localizer["TicketNotFoundErrorMessage", ticketId].Value });
            }

            return ticket != null;
        }

        private async Task CheckIsTicketAlreadyRevealedAsync(int ticketId, ICollection<ErrorViewModel> errors)
        {
            var ticket = (TicketViewModel) await _ticketRepository.GetTicketByIdAsync(ticketId);
            var isAlreadyRevealed = IsTicketRevealed(ticket);

            if (isAlreadyRevealed)
            {
                errors.Add(new ErrorViewModel { Message = _localizer["TicketAlreadyRevealedErrorMessage", ticketId].Value });
            }
        }

        private async Task<TicketEntity> SaveTicketAsync(TicketWriteModel ticket)
        {
            var ticketEntity = (TicketEntity) ticket;

            await FindEventsRatesAsync(ticketEntity);
            await FindEventResultsAsync(ticketEntity.Events);

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

        private TicketViewModel PrepareResponseTicket(TicketEntity ticket)
        {
            var result = (TicketViewModel) ticket;
            FilterTicketEventsResults(result);

            return result;
        }

        private bool VerifyTicketEvents(IEnumerable<EventEntity> events) =>
            events.All(e => e.HiddenEventResult == ResultEnum.Lost);

        private void FilterTicketEventsResults(TicketViewModel ticket)
        {
            var isRevealed = IsTicketRevealed(ticket);

            if (!isRevealed)
            {
                HideEventResult(ticket.Events);
            }
        }

        private void HideEventResult(IEnumerable<TicketEventViewModel> events)
        {
            foreach (var eventViewModel in events)
            {
                eventViewModel.Result = ResultEnum.Pending;
            }
        }

        private void SetTicketResults(bool isWon, TicketEntity ticket)
        {
            ticket.Status = isWon ? ResultEnum.Won : ResultEnum.Lost;
            ticket.RevealDateTime = DateTime.Now;
        }


        private bool IsTicketRevealed(TicketViewModel ticket) =>
            ticket.Status != ResultEnum.Pending && ticket.RevealDateTime.HasValue;

        private async Task FindEventResultsAsync(ICollection<EventEntity> events)
        {
            var gameIds = events.Select(g => g.GameId);
            var games = await _gameRepository.GetGamesByBunchOfIdsAsync(gameIds);

            foreach (var eventEntity in events)
            {
                var relatedGame = games.SingleOrDefault(g => g.GameId == eventEntity.GameId);
                var gameResult = CheckGameResult(relatedGame.GoalsHomeTeam, relatedGame.GoalsAwayTeam);

                eventEntity.HiddenEventResult = gameResult == eventEntity.Type ? ResultEnum.Won : ResultEnum.Lost;
            }
        }

        private EventType CheckGameResult(int homeTeamGoals, int awayTeamGoals)
        {
            var result = EventType.HomeTeamWin;

            if (homeTeamGoals == awayTeamGoals)
            {
                result = EventType.Tie;
            }
            else if (awayTeamGoals > homeTeamGoals)
            {
                result = EventType.AwayTeamWin;
            }

            return result;
        }

        private async Task FindEventsRatesAsync(TicketEntity ticket)
        {
            foreach (var eventEntity in ticket.Events.GetEmptyIfNull())
            {
                var rates = await _ratesRepository.GetGameRatesAsync(eventEntity.GameId);
                eventEntity.ConfirmedRate = GetCorrectRate(eventEntity.Type, rates);
            }

            ticket.TotalConfirmedRate = CalculateTotalConfirmedRate(ticket.Events);
        }

        private float GetCorrectRate(EventType type, BetRatesEntity rates) =>
            type switch
            {
                EventType.HomeTeamWin => rates.HomeTeamWinRate,
                EventType.AwayTeamWin => rates.AwayTeamWinRate,
                EventType.Tie => rates.TieRate,
                _ => throw new ArgumentException(_localizer["IncorrectEventTypeErrorMessage", type].Value, nameof(type))
            };

        private float CalculateTotalConfirmedRate(IEnumerable<EventEntity> events) =>
            (float) Math.Round(events.Select(e => e.ConfirmedRate).Product(), 2);
    }
}
