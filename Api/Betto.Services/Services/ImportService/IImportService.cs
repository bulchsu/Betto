using System.Threading.Tasks;

namespace Betto.Services
{
    public interface IImportService
    {
        Task ImportExternalDataAsync();
    }
}
