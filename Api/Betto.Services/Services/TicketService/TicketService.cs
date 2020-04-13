using System;
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
        private readonly IRatesRepository _ratesRepository;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public TicketService(ITicketRepository ticketRepository,
            IRatesRepository ratesRepository,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _ticketRepository = ticketRepository;
            _ratesRepository = ratesRepository;
            _localizer = localizer;
        }

        public async Task<TicketDTO> AddTicketAsync(CreateTicketDTO ticket)
        {
            var ticketEntity = (TicketEntity) ticket;
            await FindEventsRatesAsync(ticketEntity);

            // TODO sprawdzic czy uzytkownika stac na kupon
            var createdTicket = await _ticketRepository.AddTicketAsync(ticketEntity);
            await _ticketRepository.SaveChangesAsync();

            return (TicketDTO) createdTicket;
        }

        public async Task<ICollection<TicketDTO>> GetUserTicketsAsync(int userId) =>
            (await _ticketRepository.GetUserTicketsAsync(userId))
            .Select(t => (TicketDTO) t)
            .ToList();

        public async Task<TicketDTO> GetTicketByIdAsync(int ticketId) =>
            (TicketDTO) (await _ticketRepository.GetTicketByIdAsync(ticketId));

        public async Task<bool> CheckHasUserPlayedAnyOfGamesBeforeAsync(CreateTicketDTO ticket)
        {
            var gameIds = ticket.Events.Select(e => e.GameId);
            var userTickets = await _ticketRepository.GetUserTicketsAsync(ticket.UserId);
            var playedGames = userTickets.SelectMany(t => t.Events);

            return playedGames.Any(g => gameIds.Contains(g.GameId));
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
