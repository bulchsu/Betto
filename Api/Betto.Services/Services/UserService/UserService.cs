using System.Threading.Tasks;
using Betto.DataAccessLayer.Repositories;
using Betto.Helpers;
using Betto.Model.ViewModels;
using Betto.Model.Entities;
using Betto.Model.WriteModels;

namespace Betto.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IObjectValidator _objectValidator;

        public UserService(IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IObjectValidator objectValidator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _objectValidator = objectValidator;
        }

        public async Task<WebTokenViewModel> AuthenticateUserAsync(LoginWriteModel loginData)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginData.Username);
            var authenticated = _passwordHasher.VerifyPassword(user.PasswordHash, loginData.Password);

            return authenticated
                ? new WebTokenViewModel { AuthenticationToken = _tokenGenerator.GenerateToken(loginData.Username), Username = user.Username }
                : null;
        }

        public async Task<UserViewModel> SignUpAsync(SignUpWriteModel signUpData)
        {
            var passwordHash = _passwordHasher.EncodePassword(signUpData.Password);

            _objectValidator.ValidateObject(signUpData);

            var newUser = new UserEntity()
                { MailAddress = signUpData.MailAddress, Username = signUpData.Username, PasswordHash = passwordHash };

            var registeredUser = await _userRepository.SignUpAsync(newUser);
            await _userRepository.SaveChangesAsync();

            return (UserViewModel) registeredUser;
        }

        public async Task<UserViewModel> GetUserByUsernameAsync(string username) =>
            (UserViewModel) await _userRepository.GetUserByUsernameAsync(username);

        public async Task<bool> CheckIsUsernameAlreadyTakenAsync(string username) =>
            await _userRepository.GetUserByUsernameAsync(username) != null;

        public async Task<bool> CheckIsMailAlreadyTakenAsync(string mailAddress) =>
            await _userRepository.GetUserByMailAsync(mailAddress) != null;

        public async Task<UserViewModel> GetUserByIdAsync(int userId) =>
            (UserViewModel) await _userRepository.GetUserByIdAsync(userId);
    }
}
