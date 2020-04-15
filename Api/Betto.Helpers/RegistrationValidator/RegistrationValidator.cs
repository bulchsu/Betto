using System.Collections.Generic;
using System.Text.RegularExpressions;
using Betto.Model.Constants;
using Betto.Model.Models;
using Betto.Model.WriteModels;
using Betto.Resources.Shared;
using Microsoft.Extensions.Localization;

namespace Betto.Helpers
{
    public class RegistrationValidator : IRegistrationValidator
    {
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public RegistrationValidator(IStringLocalizer<ErrorMessages> localizer)
        {
            _localizer = localizer;
        }

        public ICollection<ErrorViewModel> ValidateRegistrationModel(RegistrationWriteModel registrationModel)
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
                errors.Add(new ErrorViewModel
                {
                    Message = _localizer["IncorrectUsernameLengthErrorMessage"]
                        .Value
                });
            }
        }

        private void ValidatePassword(string password, ICollection<ErrorViewModel> errors)
        {
            var match = Regex.Match(password, RegistrationValidatorConstants.PasswordPattern);

            if (!match.Success)
            {
                errors.Add(new ErrorViewModel
                {
                    Message = _localizer["WeakPasswordErrorMessage", 
                        RegistrationValidatorConstants.PasswordPattern]
                        .Value
                });
            }
        }

        private void ValidateMailAddress(string mailAddress, ICollection<ErrorViewModel> errors)
        {
            var match = Regex.Match(mailAddress, RegistrationValidatorConstants.MailAddressPattern);

            if (!match.Success)
            {
                errors.Add(new ErrorViewModel
                {
                    Message = _localizer["IncorrectMailAddressErrorMessage", 
                        RegistrationValidatorConstants.MailAddressPattern]
                        .Value
                });
            }
        }
    }
}
