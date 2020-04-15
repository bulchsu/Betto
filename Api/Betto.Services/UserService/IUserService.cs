using System.Threading.Tasks;
using Betto.Model.Models;
using Betto.Model.ViewModels;
using Betto.Model.WriteModels;

namespace Betto.Services
{
    public interface IUserService
    {
        Task<RequestResponseModel<WebTokenViewModel>> AuthenticateUserAsync(LoginWriteModel loginData);
        Task<RequestResponseModel<UserViewModel>> SignUpAsync(RegistrationWriteModel signUpData);
        Task<bool> CheckIsUsernameAlreadyTakenAsync(string username);
    }
}
