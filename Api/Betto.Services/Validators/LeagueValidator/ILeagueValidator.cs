using System.Threading.Tasks;

namespace Betto.Services.Validators
{
    public interface ILeagueValidator
    {
        Task<bool> CheckDoesTheLeagueExistAsync(int leagueId);
    }
}
