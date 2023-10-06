using DocumentFormat.OpenXml.Spreadsheet;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Utils;
using DriverLicenseLearningSupport.Validation;
using MailKit.Net.Imap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
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
        private readonly IMemoryCache _memoryCache;
        private readonly AppSettings _appSettings;

        public TheoryController(IImageService imageService, IQuestionService questionService
            , IAnswerService answerService, ILicenseTypeService licenseTypeService
            , IMemoryCache memoryCache, IOptionsMonitor<AppSettings> monitor)
        {
            _imageService = imageService;
            _questionService = questionService;
            _answerService = answerService;
            _licenseTypeService = licenseTypeService;
            _memoryCache = memoryCache;
            _appSettings = monitor.CurrentValue;
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
        //[Authorize(Roles = "Admin,Staff")]
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
                //await _imageService.UploadImageAsync(imageId, reqObj.imageLink);

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
        //[Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteSingleAnswer([FromRoute] int answerId)
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
        //[Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] int questionId)
        {
            //get Question to delete
            var question = await _questionService.GetByIdAsync(questionId);

            //check if question is null or not
            if (question is null)
            {
                return NotFound(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Can not found the matched question"
                });
            }
            //delete all answers of the question
            await _answerService.DeleteAnswersByQuestionIdAsync(questionId);
            //delete question
            await _questionService.DeleteQuestionAsync(questionId);

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Delete Question Successfully"
            });
        }
        

        [HttpGet]
        [Route("theory/{page:int}")]
        //[Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllQuestion([FromRoute] int page = 1)
        {
            //memory caching
            if (!_memoryCache.TryGetValue(_appSettings.TheoryCacheKey,
                out IEnumerable<QuestionWithAnswersModel> questionWithAnswersModel))
            {
                List<QuestionWithAnswersModel> result = new List<QuestionWithAnswersModel>();
                //get all questions
                var questions = await _questionService.GetAllAsync();

                foreach (QuestionModel qm in questions) 
                {
                    qm.LicenseType = await _licenseTypeService.GetAsync(qm.LicenseTypeId);
                }

                //get answers for each question
                foreach (QuestionModel question in questions)
                {
                    var answers = await _answerService.GetAllByQuestionId(question.QuestionId);
                    if (answers is null)
                    {
                        return BadRequest(new ErrorResponse()
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = "Something was wrong"
                        });
                    }
                    result.Add(new QuestionWithAnswersModel
                    {
                        question = question,
                        answers = answers
                    });
                    questionWithAnswersModel = result;
                }

                // cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // none access exceeds 45s <- remove cache
                    .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                    // after 10m from first access <- remove cache
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(600))
                    // cache priority
                    .SetPriority(CacheItemPriority.Normal);
                // set cache

                _memoryCache.Set(_appSettings.MembersCacheKey, questionWithAnswersModel, cacheEntryOptions);
            }
            else
            {
                questionWithAnswersModel = (IEnumerable<QuestionWithAnswersModel>)_memoryCache.Get(_appSettings.TheoryCacheKey);
            }
            //page size
            int pageSize = _appSettings.TheoryPageSize;
            //paging
            var list = PaginatedList<QuestionWithAnswersModel>.CreateByIEnumerable(questionWithAnswersModel, page, pageSize);

            //not found suitable question with answer
            if (questionWithAnswersModel is null)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Not found any suitable question-answers pair"
                });
            }
            return Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    QuestionWithAnswer = questionWithAnswersModel,
                    TotalPage = list.TotalPage,
                    PageIndex = list.PageIndex
                }
            });


        }
        //[HttpGet]
        //[Route("theory/")]


    }
}
// answerService AnswerModel -> QuestionID -> QuestionService
// createAsync -> questionModel -> questionId -> foreach List<AnswerModel> list -> each.QuestionId = questionId (controller)

// Repository : Tuong tac truc tiep voi db -> param : Entity, Entity Field
// Service: 