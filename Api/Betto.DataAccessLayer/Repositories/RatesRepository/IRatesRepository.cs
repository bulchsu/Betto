using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Entities;

namespace Betto.DataAccessLayer.Repositories.RatesRepository
{
    public interface IRatesRepository : IBaseRepository
    {
        Task AddRatesAsync(ICollection<BetRatesEntity> rates);
        void Clear();
    }
}
