using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.DataAccessLayer.Repositories;
using Betto.Helpers.Extensions;
using Betto.Model.Constants;
using Betto.Model.Entities;
using Betto.Model.Models;
using Betto.Model.ViewModels;
using Betto.Model.WriteModels;
using Betto.Resources.Shared;
using Microsoft.Extensions.Localization;

namespace Betto.Services.Validators
{
    public class TicketValidator : ITicketValidator
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public TicketValidator(ITicketRepository ticketRepository,
            IGameRepository gameRepository,
            IUserRepository userRepository,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _ticketRepository = ticketRepository;
            _gameRepository = gameRepository;
            _userRepository = userRepository;
            _localizer = localizer;
        }

        public async Task<bool> CheckIsTicketAlreadyRevealedAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(ticketId);
            return ticket.Status != StatusEnum.Pending || ticket.RevealDateTime != null;
        }

        public async Task<ICollection<ErrorViewModel>> ValidateTicketBeforeSavingAsync(TicketWriteModel ticket)
        {
            var errors = new List<ErrorViewModel>();

            ValidateTicketEvents(ticket, errors);
            ValidateTicketInRespectOfDuplications(ticket, errors);
            ValidateTicketStake(ticket, errors);

            var doesExist = await CheckDoesUserExistAsync(ticket.UserId);

            if (!doesExist)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["UserNotFoundErrorMessage",
                        ticket.UserId]
                    .Value));

                return errors;
            }

            await CheckDoTicketGamesExistAsync(ticket, errors);
            await ValidateUserAccountBeforeCreatingTicketAsync(ticket, errors);
            await ValidateTicketInRespectOfPreviousUserGamesAsync(ticket, errors);

            return errors;
        }

        public async Task<ICollection<ErrorViewModel>> ValidateTicketBeforeRevealingAsync(int ticketId)
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

            await ValidateTicketRevelationAsync(ticketId, errors);

            return errors;
        }

        public bool CheckIsTicketWon(TicketEntity ticket) =>
            ticket.Events.All(e => e.EventStatus == StatusEnum.Won);

        public async Task<TicketViewModel> PrepareResponseTicketAsync(TicketEntity ticket)
        {
            var result = (TicketViewModel)ticket;
            await FilterTicketEventsResultsAsync(result);

            return result;
        }

        private void ValidateTicketEvents(TicketWriteModel ticket, ICollection<ErrorViewModel> errors)
        {
            var doesContainEvents = CheckDoesTicketContainEvents(ticket);

            if (!doesContainEvents)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["LackOfEventsErrorMessage"]
                    .Value));
            }
        }

        private void ValidateTicketInRespectOfDuplications(TicketWriteModel ticket,
            ICollection<ErrorViewModel> errors)
        {
            var duplicatedGameIds = SearchForDuplicatedEventsOnATicket(ticket)
                .ToList();

            if (duplicatedGameIds.Any())
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["TicketContainsDuplicatesErrorMessage",
                        string.Join(';', duplicatedGameIds)]
                    .Value));
            }
        }

        private void ValidateTicketStake(TicketWriteModel ticket,
            ICollection<ErrorViewModel> errors)
        {
            var isStakeCorrect = CheckIsStakeCorrect(ticket);

            if (!isStakeCorrect)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["IncorrectStakeErrorMessage"]
                    .Value));
            }
        }

        private async Task CheckDoTicketGamesExistAsync(TicketWriteModel ticketModel,
            ICollection<ErrorViewModel> errors)
        {
            var incorrectGameIds = (await SearchForIncorrectGamesAsync(ticketModel))
                .ToList();

            if (incorrectGameIds.Any())
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["TicketContainingIncorrectGamesErrorMessage",
                        string.Join(';', incorrectGameIds)]
                    .Value));
            }
        }

        private async Task ValidateUserAccountBeforeCreatingTicketAsync(TicketWriteModel ticketModel,
            ICollection<ErrorViewModel> errors)
        {
            var isAffordable = await CheckIsTicketAffordableByUserAsync(ticketModel);

            if (!isAffordable)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["TicketNotAffordableErrorMessage"]
                    .Value));
            }
        }

        private async Task ValidateTicketInRespectOfPreviousUserGamesAsync(TicketWriteModel ticket, 
            ICollection<ErrorViewModel> errors)
        {
            var isTicketInvalid = await CheckHasUserPlayedAnyOfGamesBeforeAsync(ticket);

            if (isTicketInvalid)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["EventsRepeatedByUserErrorMessage"]
                    .Value));
            }
        }

        private async Task<bool> CheckDoesTicketExistAsync(int ticketId) =>
            await _ticketRepository.GetTicketByIdAsync(ticketId) != null;

        private static bool CheckDoesTicketContainEvents(TicketWriteModel ticket) =>
            ticket.Events.GetEmptyIfNull().Any();
        
        private IEnumerable<int> SearchForDuplicatedEventsOnATicket(TicketWriteModel ticket) =>
            ticket.Events
                .GroupBy(g => g.GameId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList()
                .GetEmptyIfNull();
        
        private static bool CheckIsStakeCorrect(TicketWriteModel ticket) =>
            ticket.Stake >= TicketConstants.MinimumStake && ticket.Stake <= TicketConstants.MaximumStake;
        
        private async Task<IEnumerable<int>> SearchForIncorrectGamesAsync(TicketWriteModel ticketModel)
        {
            var gameIds = ticketModel.Events.Select(e => e.GameId)
                .ToList();
            var storedGamesIds = (await _gameRepository.GetGamesByBunchOfIdsAsync(gameIds))
                .Select(g => g.GameId);
        
            var incorrectGameIds = gameIds
                .Where(g => !storedGamesIds
                    .Contains(g))
                .ToList();
        
            return incorrectGameIds;
        }
        
        private async Task<bool> CheckIsTicketAffordableByUserAsync(TicketWriteModel ticketModel)
        {
            var user = await _userRepository.GetUserByIdAsync(ticketModel.UserId, false, false);
            return user.AccountBalance >= ticketModel.Stake;
        }
        
        private async Task<bool> CheckHasUserPlayedAnyOfGamesBeforeAsync(TicketWriteModel ticket)
        {
            var gameIds = ticket.Events.Select(e => e.GameId);
            var userTickets = await _ticketRepository.GetUserTicketsAsync(ticket.UserId);
            var playedGames = userTickets.SelectMany(t => t.Events);
            var isTicketInvalid = playedGames.Any(g => gameIds.Contains(g.GameId));
        
            return isTicketInvalid;
        }

        private async Task ValidateTicketRevelationAsync(int ticketId, ICollection<ErrorViewModel> errors)
        {
            var isAlreadyRevealed = await CheckIsTicketAlreadyRevealedAsync(ticketId);

            if (isAlreadyRevealed)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["TicketAlreadyRevealedErrorMessage",
                        ticketId]
                    .Value));
            }
        }

        private async Task<bool> CheckDoesUserExistAsync(int userId) =>
            await _userRepository.GetUserByIdAsync(userId, false, false) != null;

        private async Task FilterTicketEventsResultsAsync(TicketViewModel ticket)
        {
            var isRevealed = await CheckIsTicketAlreadyRevealedAsync(ticket.TicketId);

            if (!isRevealed)
            {
                HideEventResult(ticket.Events);
            }
        }

        private static void HideEventResult(IEnumerable<TicketEventViewModel> events)
        {
            foreach (var eventViewModel in events)
            {
                eventViewModel.EventStatus = StatusEnum.Pending;
            }
        }
    }
}
