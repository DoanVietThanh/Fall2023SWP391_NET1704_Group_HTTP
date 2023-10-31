using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services.Impl;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class FeedbackController : ControllerBase 
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpDelete]
        [Route("feedbacks/{id:int}")]
        public async Task<IActionResult> DeleteFeedback([FromRoute] int id)
        {
            bool isSucess = await _feedbackService.DeleteAsync(id);

            if (isSucess)
            {
                return Ok(new BaseResponse { 
                    StatusCode =  StatusCodes.Status200OK,
                    Message = "Xóa feedback thành công"
                });
            }

            return new ObjectResult(new BaseResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = $"Xóa feedback thất bại"
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
