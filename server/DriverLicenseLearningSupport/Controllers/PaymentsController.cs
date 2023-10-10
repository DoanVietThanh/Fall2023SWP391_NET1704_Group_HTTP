using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.VnPay.Base;
using DriverLicenseLearningSupport.VnPay.Config;
using DriverLicenseLearningSupport.VnPay.Extensions;
using DriverLicenseLearningSupport.VnPay.Request;
using DriverLicenseLearningSupport.VnPay.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Options;
using System.Net;

namespace DriverLicenseLearningSupport.Controllers
{
    /// <summary>
    /// Payment api endpoints
    /// </summary>
    [Route("api/payment")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICourseReservationService _courseReservationService;
        private readonly VnPayConfig _vnPayConfig;

        /// <summary>
        /// Constructor
        /// </summary>
        public PaymentsController(IMediator mediator,
            ICourseReservationService coureReservationService,
            IOptionsMonitor<VnPayConfig> monitor)
        {
            _mediator = mediator;
            _courseReservationService = coureReservationService; 
            _vnPayConfig = monitor.CurrentValue;
        }

        /// <summary>
        /// Create payment to get link
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof (BaseResultWithData<PaymentLinkResponse>), 200)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
        {
            //// update course reservation payment status
            //await _courseReservationService.UpdateStatus(request.CourseReservationId,
            //    request.RequiredAmount);

            var reponse = new BaseResultWithData<PaymentLinkResponse>();
            reponse = await _mediator.Send(request);
            return Ok(reponse);
        }

        [HttpGet]
        [Route("vnpay-return")]
        public async Task<IActionResult> VnpayReturn([FromBody] VnpayPayResponse response)
        {
            var returnUrl = string.Empty;

            var returnModel = new PaymentReturnResponse();

            if (returnUrl.EndsWith("/")){
                returnUrl = returnUrl.Remove(returnUrl.Length - 1, 1);
            }

            return Redirect($"{returnUrl}?{returnModel.ToQueryString()}");
        }
    }
}
