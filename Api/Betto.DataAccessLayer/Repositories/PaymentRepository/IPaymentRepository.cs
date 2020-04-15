using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Entities;

namespace Betto.DataAccessLayer.Repositories.PaymentRepository
{
    public interface IPaymentRepository : IBaseRepository
    {
        Task<ICollection<PaymentEntity>> GetUserPaymentsAsync(int userId);
        Task<PaymentEntity> AddPaymentAsync(PaymentEntity payment);
    }
}