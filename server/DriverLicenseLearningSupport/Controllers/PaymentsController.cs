using DocumentFormat.OpenXml.Bibliography;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.VnPay.Base;
using DriverLicenseLearningSupport.VnPay.Config;
using DriverLicenseLearningSupport.VnPay.Extensions;
using DriverLicenseLearningSupport.VnPay.Request;
using DriverLicenseLearningSupport.VnPay.Response;
using Mapster;
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
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
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
        public async Task<IActionResult> VnpayReturn([FromQuery] VnpayPayResponse response)
        {
            var returnUrl = string.Empty;

            var returnModel = new PaymentReturnResponse();


            //update course Reservation Status
            var courseReservation = await _courseReservationService.UpdatePaymentStatusAsync(
                Guid.Parse(response?.vnp_TxnRef));

            var processResult = await _mediator.Send(response.Adapt(new ProcessVnpayResponse()));


            if (processResult.Success)
            {
                returnModel = processResult.Data.Item1 as PaymentReturnResponse;

                returnUrl = Url.Action("PaymentNotification", "Payments", string.Empty,
                    //values: new { Success = true },
                    Request.Scheme, host: "localhost:3000");
            }
            else
            {
                returnUrl = Url.Action("PaymentNotification", "Payments",
                    values: new { Sucess = false },
                    Request.Scheme, host: "localhost:3000");
            }

            //if (returnUrl.EndsWith("/")){
            //    returnUrl = returnUrl.Remove(returnUrl.Length - 1, 1);
            //}

            //return Redirect($"{returnUrl}?{returnModel.ToQueryString()}");
            return Redirect(returnUrl);
        }

        [HttpGet]
        [Route("notification")]
        public async Task<IActionResult> PaymentNotification(int statusCode, string message)
        {
            if(statusCode == 200)
            {
                return Ok(new
                {
                    Success = true,
                });
            }
            else
            {
                return BadRequest(new
                {
                    Success = false,
                });
            }
        }
    }
}
