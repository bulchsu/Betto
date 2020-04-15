using Betto.Model.Models;

namespace Betto.Model.WriteModels
{
    public class PaymentWriteModel
    {
        public int UserId { get; set; }
        public double Amount { get; set; }
        public PaymentTypeEnum Type { get; set; } 
    }
}
