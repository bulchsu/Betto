using System.Collections.Generic;
using System.Threading.Tasks;
using Betto.DataAccessLayer.Repositories;
using Betto.Model.Models;
using Betto.Model.WriteModels;
using Betto.Resources.Shared;
using Microsoft.Extensions.Localization;

namespace Betto.Services.Validators
{
    public class PaymentValidator : IPaymentValidator
    {
        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public PaymentValidator(IUserRepository userRepository,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _userRepository = userRepository;
            _localizer = localizer;
        }

        public async Task<ICollection<ErrorViewModel>> ValidatePaymentAsync(PaymentWriteModel paymentModel)
        {
             var errors = new List<ErrorViewModel>();
             var doesUserExist = await CheckDoesTheUserExistAsync(paymentModel.UserId);
             
             if (!doesUserExist)
             {
                 errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["UserNotFoundErrorMessage",
                         paymentModel.UserId]
                     .Value));
             
                 return errors;
             }
             
             if (paymentModel.Type == PaymentTypeEnum.Withdrawal)
             {
                 await CheckUserAccountBalanceBeforeWithdrawal(paymentModel, errors);
             }
             
             return errors;
        }

        private async Task CheckUserAccountBalanceBeforeWithdrawal(PaymentWriteModel paymentModel,
            ICollection<ErrorViewModel> errors)
        {
            var canWithdraw = await CheckIsUserAbleToWithdrawAsync(paymentModel);

            if (!canWithdraw)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["WithdrawalNotAffordableErrorMessage"]
                    .Value));
            }
        }

        private async Task<bool> CheckIsUserAbleToWithdrawAsync(PaymentWriteModel paymentModel)
        {
            var user = await _userRepository.GetUserByIdAsync(paymentModel.UserId, false, false);
            return user.AccountBalance >= paymentModel.Amount;
        }

        private async Task<bool> CheckDoesTheUserExistAsync(int userId) =>
            await _userRepository.GetUserByIdAsync(userId, false, false) != null;
    }
}