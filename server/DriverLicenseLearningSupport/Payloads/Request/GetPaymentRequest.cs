﻿using DriverLicenseLearningSupport.Payloads.Response;
using MediatR;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class GetPaymentRequest : IRequest<BaseResultWithData<PaymentReturnResponse>>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class GetPaymentHandler : IRequestHandler<GetPaymentRequest, BaseResultWithData<PaymentReturnResponse>>
    {
        public GetPaymentHandler()
        {

        }
        public Task<BaseResultWithData<PaymentReturnResponse>>
            Handle(GetPaymentRequest request, CancellationToken cancellationToken)
        {
            var result = new BaseResultWithData<PaymentReturnResponse>();

            try
            {

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors.Add(new BaseError()
                {
                    Message = ex.Message,
                    Code = MessageContants.Exception
                });
            }

            return Task.FromResult(result);
        }
    }
}
