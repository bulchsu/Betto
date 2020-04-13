using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Entities;

namespace Betto.DataAccessLayer.Repositories.RatesRepository
{
    public interface IRatesRepository : IBaseRepository
    {
        Task AddRatesAsync(ICollection<BetRatesEntity> rates);
        Task<BetRatesEntity> GetGameRatesAsync(int gameId);
        void Clear();
    }
}
