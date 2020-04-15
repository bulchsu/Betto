using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Betto.Model.Models;

namespace Betto.Model.Entities
{
    public class PaymentEntity
    {
        [Key]
        public int PaymentId { get; set; }
        public UserEntity User { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public double Amount { get; set; }
        public PaymentTypeEnum Type { get; set; }
        public DateTime PaymentDateTime { get; set; }
    }
}
