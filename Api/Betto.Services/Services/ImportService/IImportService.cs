using System.Threading.Tasks;

namespace Betto.Services
{
    public interface IImportService
    {
        Task ImportInitialDataAsync();
        Task ImportNextLeaguesAsync(int leaguesAmount);
    }
}
