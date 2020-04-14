﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.DataAccessLayer.Repositories;
using Betto.DataAccessLayer.Repositories.RatesRepository;
using Betto.Helpers.Extensions;
using Betto.Model.DTO;
using Betto.Model.Entities;
using Betto.Model.Models;
using Betto.Resources.Shared;
using Microsoft.Extensions.Localization;

namespace Betto.Services.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IRatesRepository _ratesRepository;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public TicketService(ITicketRepository ticketRepository,
            IGameRepository gameRepository,
            IRatesRepository ratesRepository,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _ticketRepository = ticketRepository;
            _gameRepository = gameRepository;
            _ratesRepository = ratesRepository;
            _localizer = localizer;
        }

        public async Task<TicketDTO> AddTicketAsync(CreateTicketDTO ticket)
        {
            var ticketEntity = (TicketEntity) ticket;

            await FindEventsRatesAsync(ticketEntity);
            await FindEventResultsAsync(ticketEntity.Events);

            // TODO sprawdzic czy uzytkownika stac na kupon
            var createdTicket = await _ticketRepository.AddTicketAsync(ticketEntity);
            await _ticketRepository.SaveChangesAsync();

            var result = (TicketDTO) createdTicket;
            SetProperEventResults(result);

            return result;
        }

        public async Task<ICollection<TicketDTO>> GetUserTicketsAsync(int userId)
        {
            var userTickets = (await _ticketRepository.GetUserTicketsAsync(userId))
                .Select(t => (TicketDTO) t)
                .ToList();

            foreach (var ticket in userTickets)
            {
                SetProperEventResults(ticket);
            }

            return userTickets;
        }

        public async Task<TicketDTO> GetTicketByIdAsync(int ticketId)
        { 
            var ticket = (TicketDTO) await _ticketRepository.GetTicketByIdAsync(ticketId);

            if (ticket != null)
            {
                SetProperEventResults(ticket);
            }

            return ticket;
        }

        public async Task<bool> CheckHasUserPlayedAnyOfGamesBeforeAsync(CreateTicketDTO ticket)
        {
            var gameIds = ticket.Events.Select(e => e.GameId);
            var userTickets = await _ticketRepository.GetUserTicketsAsync(ticket.UserId);
            var playedGames = userTickets.SelectMany(t => t.Events);

            return playedGames.Any(g => gameIds.Contains(g.GameId));
        }

        public async Task<TicketDTO> RevealTicketAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);

            var isAlreadyRevealed = IsTicketRevealed((TicketDTO)ticket);

            if (isAlreadyRevealed)
            {
                throw new Exception(_localizer["TicketAlreadyRevealedErrorMessage", ticketId].Value);
            }

            var isWon = VerifyTicket(ticket.Events);
            SetTicketResults(isWon, ticket);

            await _ticketRepository.SaveChangesAsync();

            var result = (TicketDTO) ticket;
            SetProperEventResults(result);

            return result;
        }

        private void SetProperEventResults(TicketDTO ticket)
        {
            var isRevealed = IsTicketRevealed(ticket);

            if (!isRevealed)
            {
                HideEventResult(ticket.Events);
            }
        }

        private void HideEventResult(ICollection<TicketEventDTO> events)
        {
            foreach (var eventDto in events)
            {
                eventDto.Result = ResultEnum.Pending;
            }
        }

        private void SetTicketResults(bool isWon, TicketEntity ticket)
        {
            ticket.Status = isWon ? ResultEnum.Won : ResultEnum.Lost;
            ticket.RevealDateTime = DateTime.Now;
        }


        private bool IsTicketRevealed(TicketDTO ticket) =>
            ticket.Status != ResultEnum.Pending && ticket.RevealDateTime.HasValue;

        private async Task FindEventResultsAsync(ICollection<EventEntity> events)
        {
            var gameIds = events.Select(g => g.GameId);
            var games = await _gameRepository.GetGamesByBunchOfIdsAsync(gameIds);

            foreach (var eventEntity in events)
            {
                var relatedGame = games.SingleOrDefault(g => g.GameId == eventEntity.GameId);
                var gameResult = CheckGameResult(relatedGame.GoalsHomeTeam, relatedGame.GoalsHomeTeam);

                eventEntity.HiddenEventResult = gameResult == eventEntity.Type ? ResultEnum.Won : ResultEnum.Lost;
            }
        }

        private bool VerifyTicket(ICollection<EventEntity> events) =>
            events.All(e => e.HiddenEventResult == ResultEnum.Lost);

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

        private float CalculateTotalConfirmedRate(ICollection<EventEntity> events) =>
            (float) Math.Round(events.Select(e => e.ConfirmedRate).Product(), 2);
    }
}
