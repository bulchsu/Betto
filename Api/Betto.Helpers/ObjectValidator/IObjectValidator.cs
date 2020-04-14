using System.Collections.Generic;
using Betto.Model.Models;

namespace Betto.Helpers
{
    public interface IObjectValidator
    {
        ICollection<ErrorViewModel> ValidateObject(object validatedInstance);
    }
}
