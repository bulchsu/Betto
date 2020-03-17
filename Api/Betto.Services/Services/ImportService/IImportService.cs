using System.Threading.Tasks;

namespace Betto.Services.Services.ImportService
{
    public interface IImportService
    {
        Task ImportExternalDataAsync();
    }
}
