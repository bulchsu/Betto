using System;
using Betto.Model.Entities;
using Betto.Model.Models;

namespace Betto.Model.ViewModels
{
    public class PaymentViewModel
    {
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public PaymentTypeEnum Type { get; set; }
        public DateTime PaymentDateTime { get; set; }

        public static explicit operator PaymentViewModel(PaymentEntity payment) => payment == null
            ? null
            : new PaymentViewModel
            {
                PaymentId = payment.PaymentId,
                UserId = payment.UserId,
                Amount = payment.Amount,
                Type = payment.Type,
                PaymentDateTime = payment.PaymentDateTime
            };
    }
}
