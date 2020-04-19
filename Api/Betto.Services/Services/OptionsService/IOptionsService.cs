using System.Threading.Tasks;
using Betto.Model.Models;
using Betto.Model.ViewModelss;

namespace Betto.Services
{
    public interface IOptionsService
    {
        Task<RequestResponseModel<InfoViewModel>> ImportInitialDataAsync();
        Task<RequestResponseModel<InfoViewModel>> ImportNextLeaguesAsync(int leaguesAmount);
    }
}
