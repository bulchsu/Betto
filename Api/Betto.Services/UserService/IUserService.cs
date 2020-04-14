using System.Threading.Tasks;
using Betto.Model.Models;
using Betto.Model.ViewModels;
using Betto.Model.WriteModels;

namespace Betto.Services
{
    public interface IUserService
    {
        Task<RequestResponse<WebTokenViewModel>> AuthenticateUserAsync(LoginWriteModel loginData);
        Task<RequestResponse<UserViewModel>> SignUpAsync(SignUpWriteModel signUpData);
        Task<bool> CheckIsUsernameAlreadyTakenAsync(string username);
    }
}
