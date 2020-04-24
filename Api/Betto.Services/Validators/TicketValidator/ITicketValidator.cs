using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Entities;
using Betto.Model.Models;
using Betto.Model.ViewModels;
using Betto.Model.WriteModels;

namespace Betto.Services.Validators
{
    public interface ITicketValidator
    {
        Task<bool> CheckIsTicketAlreadyRevealedAsync(int ticketId);
        Task<ICollection<ErrorViewModel>> ValidateTicketBeforeSavingAsync(TicketWriteModel ticket);
        Task<ICollection<ErrorViewModel>> ValidateTicketBeforeRevealingAsync(int ticketId);
        bool CheckIsTicketWon(TicketEntity ticket);
        Task<TicketViewModel> PrepareResponseTicketAsync(TicketEntity ticket);
    }
}