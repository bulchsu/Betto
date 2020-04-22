using System.Collections.Generic;
using System.Linq;
using Betto.Model.Entities;

namespace Betto.Model.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string MailAddress { get; set; }
        public double AccountBalance { get; set; }
        public ICollection<PaymentViewModel> Payments { get; set; }
        public ICollection<TicketViewModel> Tickets { get; set; }
        public static explicit operator UserViewModel(UserEntity user) => user == null
            ? null
            : new UserViewModel
            {
                UserId = user.UserId, 
                Username = user.Username, 
                MailAddress = user.MailAddress,
                AccountBalance = user.AccountBalance,
                Payments = user.Payments
                    ?.Select(p => (PaymentViewModel) p)
                    .ToList(),
                Tickets = user.Tickets
                    ?.Select(t => (TicketViewModel) t)
                    .ToList()
            };
    }
}
