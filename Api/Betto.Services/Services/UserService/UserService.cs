using System.Threading.Tasks;
using Betto.DataAccessLayer.Repositories;
using Betto.Helpers;
using Betto.Model.DTO;
using Betto.Model.Entities;

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

        public async Task<WebTokenDTO> AuthenticateUserAsync(LoginDTO loginData)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginData.Username);
            var authenticated = _passwordHasher.VerifyPassword(user.PasswordHash, loginData.Password);

            return authenticated
                ? new WebTokenDTO { AuthenticationToken = _tokenGenerator.GenerateToken(loginData.Username), Username = user.Username }
                : null;
        }

        public async Task<UserDTO> SignUpAsync(SignUpDTO signUpData)
        {
            var passwordHash = _passwordHasher.EncodePassword(signUpData.Password);

            _objectValidator.ValidateObject(signUpData);

            var newUser = new UserEntity()
                { MailAddress = signUpData.MailAddress, Username = signUpData.Username, PasswordHash = passwordHash };

            var registeredUser = await _userRepository.SignUpAsync(newUser);
            await _userRepository.SaveChangesAsync();

            return (UserDTO) registeredUser;
        }

        public async Task<UserDTO> GetUserByUsernameAsync(string username) =>
            (UserDTO) await _userRepository.GetUserByUsernameAsync(username);

        public async Task<bool> CheckIsUsernameAlreadyTakenAsync(string username) =>
            await _userRepository.GetUserByUsernameAsync(username) != null;

        public async Task<bool> CheckIsMailAlreadyTakenAsync(string mailAddress) =>
            await _userRepository.GetUserByMailAsync(mailAddress) != null;

        public async Task<UserDTO> GetUserByIdAsync(int userId) =>
            (UserDTO) await _userRepository.GetUserByIdAsync(userId);
    }
}
