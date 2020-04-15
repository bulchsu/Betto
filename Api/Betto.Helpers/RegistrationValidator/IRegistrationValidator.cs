using System.Collections.Generic;
using Betto.Model.Models;
using Betto.Model.WriteModels;

namespace Betto.Helpers
{
    public interface IRegistrationValidator
    {
        ICollection<ErrorViewModel> ValidateRegistrationModel(RegistrationWriteModel registrationModel);
    }
}
