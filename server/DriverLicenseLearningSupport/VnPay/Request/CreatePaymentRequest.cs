using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.VnPay.Base;
using DriverLicenseLearningSupport.VnPay.Config;
using DriverLicenseLearningSupport.VnPay.Contants;
using DriverLicenseLearningSupport.VnPay.Interface;
using DriverLicenseLearningSupport.VnPay.Response;
using MediatR;
using Microsoft.Extensions.Options;

namespace DriverLicenseLearningSupport.VnPay.Request
{
    public class CreatePaymentRequest : IRequest<BaseResultWithData<PaymentLinkResponse>>
    {
        public string PaymentContent { get; set; } = string.Empty;
        public string PaymentCurrency { get; set; } = string.Empty;
        public string CourseReservationId { get; set; } = string.Empty;
        public decimal? RequiredAmount { get; set; }
        //public DateTime? PaymentDate { get; set; } = DateTime.Now;
        //public DateTime? ExpiretDate { get; set; } = DateTime.Now.AddMinutes(15);
        public string? PaymentLanguage { get; set; } = string.Empty;
        public string? MemberId { get; set; } = string.Empty;
        public string? PaymentTypeDesc { get; set; } = string.Empty;   
        //public decimal? PaidAmmount { get; set; }
        //public string? PaymentStatus { get; set; } = string.Empty;
        //public string? PaymentLastMessage { get; set; } = string.Empty;
        public string? Signature { get;set ; } = string.Empty;
    }

    public class CreatePaymentHandler : IRequestHandler<CreatePaymentRequest, BaseResultWithData<PaymentLinkResponse>>
    {
        private readonly VnPayConfig _vnpayConfig;
        private readonly ICurrentUserService _currUserService;

        public CreatePaymentHandler(ICurrentUserService currUserService,IOptionsMonitor<VnPayConfig> monitor)
        {
            _vnpayConfig = monitor.CurrentValue;
            _currUserService = currUserService;
        }
        public Task<BaseResultWithData<PaymentLinkResponse>> 
            Handle(CreatePaymentRequest request, CancellationToken cancellationToken)
        {
            var result = new BaseResultWithData<PaymentLinkResponse>();

            try
            {
                var paymentUrl = string.Empty;

                switch (request.PaymentTypeDesc)
                {
                    case "VNPAY":
                        var vnpayPayRequest = new VnpayPayRequest(_vnpayConfig.Version,
                                _vnpayConfig.TmnCode, DateTime.Now, _currUserService.IpAddress ?? string.Empty, request.RequiredAmount ?? 0, request.PaymentCurrency ?? string.Empty,
                                "other", request.PaymentContent ?? string.Empty, _vnpayConfig.ReturnUrl, request.CourseReservationId ?? string.Empty);
                        paymentUrl = vnpayPayRequest.GetLink(_vnpayConfig.PaymentUrl, _vnpayConfig.HashSecret);
                        break;
                    case "Momo": break;
                    default: break;
                }

                result.Set(true, MessageContants.OK, new PaymentLinkResponse {
                    PaymentId = Guid.NewGuid().ToString(),
                    PaymentUrl = paymentUrl
                });

            }catch(Exception ex) { 
                result.Success = false;
                result.Errors.Add(new BaseError()
                {
                    Message = ex.Message
                });
            }

            return Task.FromResult(result);
        }
    }
}
