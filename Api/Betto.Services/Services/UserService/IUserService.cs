using System.Threading.Tasks;
using Betto.Model.ViewModels;
using Betto.Model.WriteModels;

namespace Betto.Services
{
    public interface IUserService
    {
        Task<WebTokenViewModel> AuthenticateUserAsync(LoginWriteModel loginData);
        Task<UserViewModel> SignUpAsync(SignUpWriteModel signUpData);
        Task<UserViewModel> GetUserByUsernameAsync(string username);
        Task<bool> CheckIsUsernameAlreadyTakenAsync(string username);
        Task<bool> CheckIsMailAlreadyTakenAsync(string mailAddress);
        Task<UserViewModel> GetUserByIdAsync(int userId);
    }
}
