using System.Threading.Tasks;
using Betto.Model.Models;
using Betto.Model.ViewModels;
using Betto.Model.WriteModels;

namespace Betto.Services
{
    public interface IPaymentService
    {
        Task<RequestResponseModel<PaymentViewModel>> CreatePaymentAsync(PaymentWriteModel paymentModel);
    }
}