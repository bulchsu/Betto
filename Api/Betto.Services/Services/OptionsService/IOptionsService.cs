using System.Threading.Tasks;
using Betto.Model.Models;
using Betto.Model.ViewModelss;

namespace Betto.Services
{
    public interface IOptionsService
    {
        Task<RequestResponse<InfoViewModel>> ImportInitialDataAsync();
        Task<RequestResponse<InfoViewModel>> ImportNextLeaguesAsync(int leaguesAmount);
    }
}
