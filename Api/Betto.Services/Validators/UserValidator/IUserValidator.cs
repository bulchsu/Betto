using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Models;
using Betto.Model.WriteModels;

namespace Betto.Services.Validators
{
    public interface IUserValidator
    {
        Task<bool> CheckIsUsernameAlreadyTakenAsync(string username);
        Task<ICollection<ErrorViewModel>> CheckLoginCredentials(LoginWriteModel loginModel);
        Task<ICollection<ErrorViewModel>> CheckSignUpDataBeforeRegisteringAsync(RegistrationWriteModel signUpModel);
        Task<string> GetUserRoleBeforeRegisterAsync();
    }
}