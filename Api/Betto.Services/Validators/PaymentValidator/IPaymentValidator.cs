using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Models;
using Betto.Model.WriteModels;

namespace Betto.Services.Validators
{
    public interface IPaymentValidator
    {
        Task<ICollection<ErrorViewModel>> ValidatePaymentAsync(PaymentWriteModel paymentModel);
    }
}