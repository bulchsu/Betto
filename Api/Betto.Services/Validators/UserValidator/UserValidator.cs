using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Betto.DataAccessLayer.Repositories;
using Betto.Helpers;
using Betto.Model.Constants;
using Betto.Model.Models;
using Betto.Model.WriteModels;
using Betto.Resources.Shared;
using Microsoft.Extensions.Localization;

namespace Betto.Services.Validators
{
    public class UserValidator : IUserValidator
    {
        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<ErrorMessages> _localizer;
        private readonly IPasswordHasher _passwordHasher;

        public UserValidator(IUserRepository userRepository,
            IStringLocalizer<ErrorMessages> localizer,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _localizer = localizer;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> CheckDoesTheUserExistAsync(int userId) =>
            await _userRepository.GetUserByIdAsync(userId, false, false) != null;

        public async Task<bool> CheckIsUsernameAlreadyTakenAsync(string username) =>
            await _userRepository.GetUserByUsernameAsync(username) != null;

        public async Task<ICollection<ErrorViewModel>> CheckLoginCredentials(LoginWriteModel loginModel)
        {
            var errors = await CheckDoesTheUserExistAsync(loginModel.Username);

            if (!errors.Any())
            {
                await VerifyUserPasswordAsync(loginModel, errors);
            }

            return errors;
        }

        public async Task<ICollection<ErrorViewModel>> CheckSignUpDataBeforeRegisteringAsync(
            RegistrationWriteModel signUpModel)
        {
            var errors = ValidateRegistrationModel(signUpModel);

            if (!errors.Any())
            {
                await CheckUsernameBeforeSignUpAsync(signUpModel.Username, errors);
                await CheckMailAddressBeforeSignUpAsync(signUpModel.MailAddress, errors);
            }

            return errors;
        }

        private async Task<ICollection<ErrorViewModel>> CheckDoesTheUserExistAsync(string username)
        {
            var errors = new List<ErrorViewModel>();
            var doesExist = await CheckIsUsernameAlreadyTakenAsync(username);

            if (!doesExist)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["UserNotFoundErrorMessage",
                        username]
                    .Value));
            }

            return errors;
        }

        private async Task VerifyUserPasswordAsync(LoginWriteModel loginModel, ICollection<ErrorViewModel> errors)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginModel.Username);
            var authenticated = _passwordHasher.VerifyPassword(user.PasswordHash, loginModel.Password);

            if (!authenticated)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["IncorrectPasswordErrorMessage"]
                    .Value));
            }
        }

        private async Task CheckUsernameBeforeSignUpAsync(string username, ICollection<ErrorViewModel> errors)
        {
            var doesExist = await CheckIsUsernameAlreadyTakenAsync(username);

            if (doesExist)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["UsernameAlreadyTakenErrorMessage",
                        username]
                    .Value));
            }
        }

        private async Task CheckMailAddressBeforeSignUpAsync(string mailAddress, ICollection<ErrorViewModel> errors)
        {
            var isMailTaken = await CheckIsMailAlreadyTakenAsync(mailAddress);

            if (isMailTaken)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["MailAddressAlreadyTakenErrorMessage",
                        mailAddress]
                    .Value));
            }
        }

        private ICollection<ErrorViewModel> ValidateRegistrationModel(RegistrationWriteModel registrationModel)
        {
            var errors = new List<ErrorViewModel>();

            ValidateUsername(registrationModel.Username, errors);
            ValidatePassword(registrationModel.Password, errors);
            ValidateMailAddress(registrationModel.MailAddress, errors);

            return errors;
        }

        private void ValidateUsername(string username, ICollection<ErrorViewModel> errors)
        {
            if (username.Length < RegistrationValidatorConstants.MinimumUsernameLength ||
                username.Length > RegistrationValidatorConstants.MaximumUsernameLength)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["IncorrectUsernameLengthErrorMessage"]
                    .Value));
            }
        }

        private void ValidatePassword(string password, ICollection<ErrorViewModel> errors)
        {
            var match = Regex.Match(password, RegistrationValidatorConstants.PasswordPattern);

            if (!match.Success)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["WeakPasswordErrorMessage",
                        RegistrationValidatorConstants.PasswordPattern]
                    .Value));
            }
        }

        private void ValidateMailAddress(string mailAddress, ICollection<ErrorViewModel> errors)
        {
            var match = Regex.Match(mailAddress, RegistrationValidatorConstants.MailAddressPattern);

            if (!match.Success)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["IncorrectMailAddressErrorMessage",
                        RegistrationValidatorConstants.MailAddressPattern]
                    .Value));
            }
        }

        private async Task<bool> CheckIsMailAlreadyTakenAsync(string mailAddress) =>
            await _userRepository.GetUserByMailAsync(mailAddress) != null;
    }
}
