using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Validation;
using MailKit.Net.Imap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class TheoryController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly ILicenseTypeService _licenseTypeService;

        public TheoryController(IImageService imageService, IQuestionService questionService
            , IAnswerService answerService, ILicenseTypeService licenseTypeService)
        {
            _imageService = imageService;
            _questionService = questionService;
            _answerService = answerService;
            _licenseTypeService = licenseTypeService;
        }


        [HttpGet]
        [Route("theory/license-form")]
        public async Task<IActionResult> LicenseFormRegister()
        {
            var licenseTypes = await _licenseTypeService.GetAllAsync();

            if (licenseTypes is null) return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Something went wrong"
            });

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = licenseTypes
            });
        }



        [HttpPost]
        [Route("theory/add-question")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddQuestion([FromForm] CreateNewQuestionRequest reqObj)
        {


            // generate model
            var questionModel = reqObj.ToQuestionModel();

            //checkValidation of questionModel
            QuestionValidator questionValidator = new QuestionValidator();
            var checkquestionModel = await questionValidator.ValidateAsync(questionModel);
            if (!checkquestionModel.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = checkquestionModel
                });
            }


            if (reqObj.imageLink is not null)
            {
                //check validaiton of picture input from the request
                var imageFileValidator = new Validation.CreateNewQuestionValidator();
                var ImageValidatorresult = await imageFileValidator.ValidateAsync(reqObj);
                if (!ImageValidatorresult.IsValid) return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ImageValidatorresult.ToProblemDetails()
                });
                //   generate guid id
                var imageId = Guid.NewGuid();
                //upload image to cloud
                await _imageService.UploadImageAsync(imageId, reqObj.imageLink);

                //Set image Id to question model 
                questionModel.Image = imageId.ToString();
            }

            // create question

            var createdQuestionModel = await _questionService.CreateAsync(questionModel);
            if (createdQuestionModel is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //Set license type to quesiton model
            createdQuestionModel.LicenseType = await _licenseTypeService.GetAsync(Convert.ToInt32(createdQuestionModel.LicenseTypeId));



            List<AnswerModel> list = reqObj.ToListAnswerModel();
            AnswerValidator answerValidator = new AnswerValidator();
            foreach (AnswerModel model in list)
            {
                //validate answer model
                var checkAnswerModel = await answerValidator.ValidateAsync(model);
                if (!checkAnswerModel.IsValid)
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Errors = checkAnswerModel
                    });
                }
                //generate answer model
                model.QuestionId = createdQuestionModel.QuestionId;
            }


            //create answer model with full attribute
            var createdAnswerModel = await _answerService.CreateRangeAsync(list);

            if (createdAnswerModel is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


            return Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                //Data = createdQuestionModel,
                Data = new
                {
                    Question = createdQuestionModel,
                    Answers = createdAnswerModel,
                },
                Message = "Create successfully"
            });
        }


        [HttpDelete]
        [Route("theory/{answerId:int}/delete-answer")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteSingleAnswer([FromRoute]int answerId)
        {
            bool isSucess = await _answerService.DeleteAnswerAsync(answerId);
            if (!isSucess) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Delete answer succesfully"
            });
        }

        [HttpDelete]
        [Route("theory/{questionId:int}/delete-question")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] int questionId) 
        {
               
        }
        
    }
}
// answerService AnswerModel -> QuestionID -> QuestionService
// createAsync -> questionModel -> questionId -> foreach List<AnswerModel> list -> each.QuestionId = questionId (controller)

// Repository : Tuong tac truc tiep voi db -> param : Entity, Entity Field
// Service: 