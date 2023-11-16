using DocumentFormat.OpenXml.Bibliography;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services.Impl;
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
        private readonly ICoursePackageReservationService _courseReservationService;
        private readonly VnPayConfig _vnPayConfig;
        private readonly ProfileConfig _profileConfig;

        /// <summary>
        /// Constructor
        /// </summary>
        public PaymentsController(IMediator mediator,
            ICoursePackageReservationService coureReservationService,
            IOptionsMonitor<VnPayConfig> monitor,
            IOptionsMonitor<ProfileConfig> monitor1)
        {
            _mediator = mediator;
            _courseReservationService = coureReservationService; 
            _vnPayConfig = monitor.CurrentValue;
            _profileConfig = monitor1.CurrentValue;
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

            var processResult = await _mediator.Send(response.Adapt(new ProcessVnpayResponse()));

            if (processResult.Success)
            {
                returnModel = processResult.Data.Item1 as PaymentReturnResponse;

                //update course Reservation Status
                await _courseReservationService.UpdatePaymentStatusAsync(
                    Guid.Parse(response?.vnp_TxnRef),
                    Convert.ToDouble(response.vnp_Amount));

                returnUrl = Url.Action("PaymentNotification", "Payments",
                    //string.Empty,
                    values: new { Success = true },
                    Request.Scheme, host: _profileConfig.VercelDeployUrl);
            }
            else
            {
                returnUrl = Url.Action("PaymentNotification", "Payments",
                    //string.Empty,
                    values: new { Sucess = false },
                    Request.Scheme, host: _profileConfig.VercelDeployUrl);
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
