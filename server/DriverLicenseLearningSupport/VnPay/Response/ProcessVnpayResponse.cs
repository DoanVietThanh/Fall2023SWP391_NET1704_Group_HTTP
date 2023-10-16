using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.VnPay.Base;
using DriverLicenseLearningSupport.VnPay.Config;
using DriverLicenseLearningSupport.VnPay.Contants;
using MediatR;
using Microsoft.Extensions.Options;
using OfficeOpenXml.ConditionalFormatting;

namespace DriverLicenseLearningSupport.VnPay.Response
{

    public class ProcessVnpayResponse : VnpayPayResponse,
        IRequest<BaseResultWithData<(PaymentReturnResponse, string)>>
    {

    }

    public class ProcessVnpayResponseHandler
        : IRequestHandler<ProcessVnpayResponse, BaseResultWithData<(PaymentReturnResponse, string)>>
    {
        private readonly VnPayConfig _vnpayConfig;
        private readonly ICoursePackageReservationService _courseReservationService;

        public ProcessVnpayResponseHandler(IOptionsMonitor<VnPayConfig> monitor,
            ICoursePackageReservationService courseReservationService)
        {
            _vnpayConfig = monitor.CurrentValue;
            _courseReservationService = courseReservationService;
        }

        public Task<BaseResultWithData<(PaymentReturnResponse, string)>> 
            Handle(ProcessVnpayResponse request, CancellationToken cancellationToken)
        {

            var returnUrl = string.Empty;
            var result = new BaseResultWithData<(PaymentReturnResponse, string)>();
            try
            {
                var resultData = new PaymentReturnResponse();
                var isValidSignature = request.IsValidSignature(_vnpayConfig.HashSecret);
                if (isValidSignature)
                {
                    if (request.vnp_ResponseCode == "00")
                    {
                        resultData.PaymentStatus = "00";
                        resultData.PaymentId = Guid.NewGuid().ToString();
                        /// TODO: Make signature
                        resultData.Signature = Guid.NewGuid().ToString();

                        // response
                        result.Success = true;
                        result.Message = MessageContants.OK;
                        result.Data = (resultData, returnUrl);
                    }
                    else
                    {
                        resultData.PaymentStatus = "10";
                        resultData.PaymentMessage = "Payment proccess failed";
                    }
                }
                else
                {
                    resultData.PaymentStatus = "99";
                    resultData.PaymentMessage = "Invalid signature in response";
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Errors.Add(new BaseError
                {
                    Code = MessageContants.Error,
                    Message = ex.Message
                });
            }
            return Task.FromResult(result);
        }
    }
}
