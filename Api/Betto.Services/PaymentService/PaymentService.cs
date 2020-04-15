using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betto.DataAccessLayer.Repositories;
using Betto.DataAccessLayer.Repositories.PaymentRepository;
using Betto.Helpers.Extensions;
using Betto.Model.Entities;
using Betto.Model.Models;
using Betto.Model.ViewModels;
using Betto.Model.WriteModels;
using Betto.Resources.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Betto.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public PaymentService(IPaymentRepository paymentRepository,
            IUserRepository userRepository,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _localizer = localizer;
        }

        public async Task<RequestResponseModel<ICollection<PaymentViewModel>>> GetUserPaymentsAsync(int userId)
        {
            var doesUserExist = await CheckDoesUserExistAsync(userId);

            if (!doesUserExist)
            {
                return new RequestResponseModel<ICollection<PaymentViewModel>>(StatusCodes.Status404NotFound,
                    new List<ErrorViewModel>
                    {
                        ErrorViewModel.Factory.NewErrorFromMessage(_localizer["UserNotFoundErrorMessage",
                                userId]
                            .Value)
                    },
                    null);
            }

            var payments = (await _paymentRepository.GetUserPaymentsAsync(userId))
                .Select(p => (PaymentViewModel)p)
                .ToList()
                .GetEmptyIfNull();

            return new RequestResponseModel<ICollection<PaymentViewModel>>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(),
                payments);
        }

        public async Task<RequestResponseModel<PaymentViewModel>> CreatePaymentAsync(PaymentWriteModel paymentModel)
        {
            var errors = await ValidatePaymentAsync(paymentModel);

            if (errors.Any())
            {
                return new RequestResponseModel<PaymentViewModel>(StatusCodes.Status400BadRequest,
                    errors,
                    null);
            }

            var createdPayment = await SavePaymentAsync(paymentModel);

            if (createdPayment == null)
            {
                return new RequestResponseModel<PaymentViewModel>(StatusCodes.Status404NotFound,
                    new List<ErrorViewModel>
                    {
                        ErrorViewModel.Factory.NewErrorFromMessage(_localizer["PaymentNotCreatedErrorMessage",
                                paymentModel.UserId]
                            .Value)
                    }, null);
            }

            var result = (PaymentViewModel) createdPayment;

            return new RequestResponseModel<PaymentViewModel>(StatusCodes.Status200OK,
                Enumerable.Empty<ErrorViewModel>(),
                result);
        }

        private async Task<PaymentEntity> SavePaymentAsync(PaymentWriteModel paymentModel)
        {
            var payment = PreparePaymentEntityToSave(paymentModel);

            var paymentEntity = await _paymentRepository.AddPaymentAsync(payment);
            await _paymentRepository.SaveChangesAsync();

            return paymentEntity;
        }

        private async Task<ICollection<ErrorViewModel>> ValidatePaymentAsync(PaymentWriteModel paymentModel)
        {
            var errors = new List<ErrorViewModel>();
            var doesUserExist = await CheckDoesUserExistAsync(paymentModel.UserId);

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

        private async Task<bool> CheckDoesUserExistAsync(int userId) =>
            await _userRepository.GetUserByIdAsync(userId) != null;

        private async Task CheckUserAccountBalanceBeforeWithdrawal(PaymentWriteModel paymentModel,
            ICollection<ErrorViewModel> errors)
        {
            var user = await _userRepository.GetUserByIdAsync(paymentModel.UserId);

            if (paymentModel.Amount > user.AccountBalance)
            {
                errors.Add(ErrorViewModel.Factory.NewErrorFromMessage(_localizer["WithdrawalNotAffordableErrorMessage"]
                    .Value));
            }
        }

        private static PaymentEntity PreparePaymentEntityToSave(PaymentWriteModel paymentModel)
        {
            var paymentEntity = (PaymentEntity) paymentModel;
            paymentEntity.PaymentDateTime = DateTime.Now;

            return paymentEntity;
        }
    }
}