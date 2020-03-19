using System.Threading.Tasks;

namespace Betto.DataAccessLayer.Repositories.BaseRepository
{
    public interface IBaseRepository
    {
        Task<int> SaveChangesAsync();
    }
}
