using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.Model.Models;
using Betto.Model.ViewModels;
using Betto.Model.WriteModels;

namespace Betto.Services
{
    public interface IUserService
    {
        Task<RequestResponseModel<AuthenticationViewModel>> AuthenticateUserAsync(LoginWriteModel loginData);
        Task<RequestResponseModel<UserViewModel>> SignUpAsync(RegistrationWriteModel signUpData);
        Task<RequestResponseModel<ICollection<UserRankingPositionViewModel>>> GetUsersRankingAsync();
        Task<RequestResponseModel<UserViewModel>> GetUserByIdAsync(int userId, bool includePayments,
            bool includeTickets);
    }
}
