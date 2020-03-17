using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories
{
    public interface IBaseRepository
    {
        Task<int> SaveChangesAsync();
    }
}
