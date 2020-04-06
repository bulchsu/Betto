using System.Threading.Tasks;
using Betto.Model.DTO;

namespace Betto.Services
{
    public interface IUserService
    {
        Task<WebTokenDTO> AuthenticateUserAsync(LoginDTO loginData);
        Task<UserDTO> SignUpAsync(SignUpDTO signUpData);
        Task<UserDTO> GetUserByUsernameAsync(string username);
        Task<bool> CheckIsUsernameAlreadyTakenAsync(string username);
        Task<bool> CheckIsMailAlreadyTakenAsync(string mailAddress);
        Task<UserDTO> GetUserByIdAsync(int userId);
    }
}
