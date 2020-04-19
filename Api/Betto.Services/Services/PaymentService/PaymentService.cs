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
using Betto.Services.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Betto.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserValidator _userValidator;
        private readonly IPaymentValidator _paymentValidator;
        private readonly IStringLocalizer<ErrorMessages> _localizer;

        public PaymentService(IPaymentRepository paymentRepository,
            IUserRepository userRepository,
            IUserValidator userValidator,
            IPaymentValidator paymentValidator,
            IStringLocalizer<ErrorMessages> localizer)
        {
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _userValidator = userValidator;
            _paymentValidator = paymentValidator;
            _localizer = localizer;
        }

        public async Task<RequestResponseModel<ICollection<PaymentViewModel>>> GetUserPaymentsAsync(int userId)
        {
            var doesUserExist = await _userValidator.CheckDoesTheUserExistAsync(userId);

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
            var errors = await _paymentValidator.ValidatePaymentAsync(paymentModel);

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
                        ErrorViewModel.Factory.NewErrorFromMessage(_localizer["PaymentNotCreatedErrorMessage"]
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

            await ChargeUserAccountBalanceAsync(paymentModel);
            var paymentEntity = await _paymentRepository.AddPaymentAsync(payment);
            await _paymentRepository.SaveChangesAsync();

            return paymentEntity;
        }

        private static PaymentEntity PreparePaymentEntityToSave(PaymentWriteModel paymentModel)
        {
            var paymentEntity = (PaymentEntity) paymentModel;
            paymentEntity.PaymentDateTime = DateTime.Now;

            return paymentEntity;
        }

        private async Task ChargeUserAccountBalanceAsync(PaymentWriteModel paymentModel)
        {
            var user = await _userRepository.GetUserByIdAsync(paymentModel.UserId);

            user.AccountBalance +=
                paymentModel.Type == PaymentTypeEnum.Deposit 
                    ? Math.Round(paymentModel.Amount, 2) 
                    : Math.Round(-paymentModel.Amount, 2);
        }
    }
}