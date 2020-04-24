using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Betto.DataAccessLayer.Repositories.PaymentRepository
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        public PaymentRepository(BettoDbContext dbContext)
            : base(dbContext)
        {
            
        }

        public async Task<ICollection<PaymentEntity>> GetUserPaymentsAsync(int userId) =>
            await DbContext.Payments
                .Where(p => p.UserId == userId)
                .ToListAsync();

        public async Task<PaymentEntity> AddPaymentAsync(PaymentEntity payment) =>
            (await DbContext.AddAsync(payment)).Entity;
    }
}