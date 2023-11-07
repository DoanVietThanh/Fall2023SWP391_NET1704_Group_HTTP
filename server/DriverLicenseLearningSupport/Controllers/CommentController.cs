using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DriverLicenseLearningSupport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IImageService _imageService;
        private ICommentService _commentService;
            
        public CommentController(ICommentService commentService, IImageService imageService)
        {
            _imageService = imageService;
            _commentService = commentService;
        }

        [HttpPost]
        [Route("/comment")]

        public async Task<IActionResult> CreateCommentAsync([FromBody] CreateNewCommentRequest reqObj)
        {
            var comment = reqObj.ToCommentModel();

            CommentValidator validation = new CommentValidator();
            var checkValidation = await validation.ValidateAsync(comment);
            if (!checkValidation.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Điền lại form"
                });
            }

            
            comment.AvatarImage = reqObj.AvatarImage;

            var createdComment = await _commentService.CreateAsync(comment);
            if (createdComment is null)
            {
                return new ObjectResult(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Tạo thất bại"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            
            return Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = createdComment,
                Message = "Bình luận thành công"
            }) ;

        }

        [HttpDelete]
        [Route("/comment/{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id) 
        {
            bool isSuccess = await _commentService.DeleteAsync(id);
            if (isSuccess) 
            {
                return Ok(new BaseResponse()
                {
                    Message="Xóa bình luận thành công",
                    StatusCode = StatusCodes.Status200OK
                });
            }
            else 
            {
                return new ObjectResult(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Xóa bình luận thất bại"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpPut]
        [Route("/comment")]
        public async Task<IActionResult> UpdateCommentAsync([FromBody] UpdateCommentRequest reqObj) 
        {
            CommentModel model = new CommentModel();
            model.CommentId = reqObj.CommentId;
            model.Content = reqObj.Content;
            bool isSuccess= await _commentService.UpdateAsync(model);
            if (isSuccess) 
            {
                return Ok(new BaseResponse() 
                {
                    Message = "Cập nhật bình luận thành công",
                    StatusCode = StatusCodes.Status200OK
                });
            }
            else {
                return new ObjectResult(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Cập nhật bình luận thất bại"
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

        }

    }
}
