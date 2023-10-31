using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class RollCallBookController : ControllerBase
    {
        private readonly IRollCallBookService _rollCallBookService;
        private readonly IEmailService _emailService;
        private readonly IMemberService _memberService;
        private readonly ITeachingScheduleService _teachingScheduleService;

        public RollCallBookController(IRollCallBookService rollCallBookService,
            ITeachingScheduleService teachingScheduleService,
            IMemberService memberService,
            IEmailService emailService)
        {
            _rollCallBookService = rollCallBookService;
            _emailService = emailService;
            _memberService = memberService;
            _teachingScheduleService = teachingScheduleService;
        }


        /// <summary>
        /// Cancel learning schedule
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("rollcallbooks/{id:int}/cancel")]
        //[Authorize(Roles = "Member")]
        public async Task<IActionResult> CancelLearningSchedule([FromRoute] int id,
            [FromQuery] string cancelMessage)
        {
            // get rollcallbook by id
            var rcbook = await _rollCallBookService.GetAsync(id);
            if (rcbook is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không tìm thấy lịch học"
                });
            }

            // check already learn
            if(rcbook.IsAbsence == false)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Bạn đã học buổi này, không thể yêu hủy"
                });
            }

            // update roll call book status
            bool isSuccess = await _rollCallBookService.UpdateInActiveStatusAsync(rcbook.RollCallBookId,
                cancelMessage);

            if (isSuccess)
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Yêu cầu hủy lịch học của bạn được gửi thành công," +
                    " vui lòng chờ phản hồi"
                });
            }
            else
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Lịch học đã diễn ra không thể hủy"
                });
            }
        }


        /// <summary>
        /// Get all inactive roll call book
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("rollcallbooks/cancel")]
        //[Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> GetAllInActiveRollCallBook()
        {
            var awaitRcbs = await _rollCallBookService.GetAllInActiveRollCallBookAsync();
            if (awaitRcbs.Count() > 0)
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = awaitRcbs
                });
            }

            return BadRequest(new BaseResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Không tìm thấy yêu cầu hủy lịch học nào"
            });
        }


        /// <summary>
        /// Approve cancel schedule
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("rollcallbooks/{id:int}/approve-cancel")]
        public async Task<IActionResult> ApproveCancelSchedule([FromRoute] int id)
        {
            // get rollcallbook by id
            var rcb = await _rollCallBookService.GetAsync(id);
            // not found
            if(rcb is null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không tìm thấy lịch học"
                });
            }
            
            // get member by id
            var member = await _memberService.GetAsync(
                                    Guid.Parse(rcb.MemberId));

            // get teaching schedule by id
            var teachingSchedule = await _teachingScheduleService.GetAsync(
                rcb.TeachingScheduleId);


            // approve cancel
            bool isApproved = await _rollCallBookService.ApproveCancelAsync(id);

            // send success email
            if (isApproved)
            {
                var message = new EmailMessage(new string[] { member.Email},
                    "Yêu cầu hủy lịch học",
                    $"Lịch học của bạn ngày {teachingSchedule.TeachingDate.ToString("dd/MM/yyyy")} " +
                    $"đã được hủy thành công.");

                //_emailService.SendEmail(message);

                return Ok(new BaseResponse { 
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Xác nhận hủy lịch học thành công"
                });
            }

            return BadRequest(new BaseResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Xác nhận hủy lịch học thất bại"
            });
        }


        /// <summary>
        /// Deny cancel schedule
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("rollcallbooks/{id:int}/deny-cancel")]
        public async Task<IActionResult> DenyCancelSchedule([FromRoute] int id,
            string? message)
        {
            // get rollcallbook by id
            var rcb = await _rollCallBookService.GetAsync(id);
            // not found
            if (rcb is null)
            {
                return BadRequest(new BaseResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không tìm thấy lịch học"
                });
            }

            // get member by id
            var member = await _memberService.GetAsync(
                                    Guid.Parse(rcb.MemberId));

            // get teaching schedule by id
            var teachingSchedule = await _teachingScheduleService.GetAsync(
                rcb.TeachingScheduleId);

            // check exist teaching schedule
            if(teachingSchedule is null)
            {
                return BadRequest(new BaseResponse { 
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không tìm thấy lịch học"
                });
            }

            // update status
            bool isDenied = await _rollCallBookService.DenyCancelSchedule(rcb.RollCallBookId);
            if (isDenied)
            {
                // send schedule deny email
                var emailMessage = new EmailMessage(new string[] { member.Email! }, "Yêu cầu hủy lịch học đã bị từ chối",
                    $"Lịch học ngày {teachingSchedule.TeachingDate.ToString("dd/MM/yyyy")}" +
                    $"Lý do từ chối: \n" +
                    message + "\n Xin cảm ơn.");
                //_emailService.SendEmail(emailMessage);

                return Ok(new BaseResponse { 
                    StatusCode = StatusCodes.Status200OK,
                    Message = $"Từ chối hủy lịch học thành công"
                });
            }

            return new ObjectResult(new BaseResponse {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Có lỗi xảy ra"
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

    }
}
