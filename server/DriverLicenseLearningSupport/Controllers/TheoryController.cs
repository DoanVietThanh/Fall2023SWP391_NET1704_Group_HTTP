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
using System.Runtime.InteropServices;

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
        private readonly ITheoryExamService _theoryExamService;
        private readonly TheoryExamSettings _theoryExamSettings;

        public TheoryController(IImageService imageService, IQuestionService questionService
            , IAnswerService answerService, ILicenseTypeService licenseTypeService
            , IMemoryCache memoryCache, IOptionsMonitor<AppSettings> monitor,
            ITheoryExamService theoryExamService,
            IOptionsMonitor<TheoryExamSettings> monitor1)
        {
            _imageService = imageService;
            _questionService = questionService;
            _answerService = answerService;
            _licenseTypeService = licenseTypeService;
            _memoryCache = memoryCache;
            _appSettings = monitor.CurrentValue;
            _theoryExamService = theoryExamService;
            _theoryExamSettings = monitor1.CurrentValue;
        }


        [HttpGet]
        [Route("theory/add-question")]
         // [Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> LicenseFormRegister()
        {
            var licenseTypes = await _licenseTypeService.GetAllAsync();

            if (licenseTypes is null) return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Đã xảy ra lỗi"
            });

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = licenseTypes,
                Message = "Thành công"
            });
        }


        [HttpPut]
        [Route("theory/update-question")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> UpdateQuestion([FromBody] UpdateQuestionRequest reqObj)
        {
            if (await _theoryExamService.IsExamQuestion(reqObj.QuestionID))
            {
                return BadRequest(new ErrorResponse {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Câu hỏi đã trong đề thi, không thể xóa"
                });
            }
            var currentQuestion = await _questionService.GetByIdAsync(reqObj.QuestionID);
            
            int licenseID = currentQuestion.LicenseTypeId;
            if (await _questionService.CheckExistedQuestion(reqObj.QuestionAnswerDesc, currentQuestion.LicenseTypeId))
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không được tạo trùng câu hỏi"
                });
            }


            var questionToUpdate = reqObj.toQuestionModel();


            var updatedQuestion = await _questionService.UpdateQuestionAsync(questionToUpdate, reqObj.QuestionID);
            

            if (updatedQuestion is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            foreach (AnswerModel answer in reqObj.Answers)
            {
                var updatedAnswer = await _answerService.UpdateAnswerAsync(answer.QuestionAnswerId, answer);
                if (updatedAnswer is null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return Ok(new BaseResponse() {
                StatusCode = StatusCodes.Status200OK,
                Message = "Cập nhật thành công"
            });


        }


        [HttpPost]
        [Route("theory/add-question")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddQuestion([FromForm] NewQuestionAddRequest reqObj)
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
                    Errors = checkquestionModel,
                    Message = "Điền lại form"
                    
                });
            }
            if (await _questionService.CheckExistedQuestion(questionModel.QuestionAnswerDesc, questionModel.LicenseTypeId))
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Không được tạo trùng câu hỏi"
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

            //set status to question model
            createdQuestionModel = await _questionService.UpdateStatusQuestionAsync(createdQuestionModel.QuestionId, true);

            List<AnswerModel> list = reqObj.ToListAnswerModel();
            if (list.Count() >= 5)
            {
                return BadRequest(new ErrorResponse() {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Số lượng câu hỏi dưới 5"
                });
            }
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
        public async Task<IActionResult> DeleteSingleAnswer([FromRoute] int answerId)
        {
            // get answer
            AnswerModel answer = await _answerService.GetByAnswerIdAsync(answerId);
            //check the existance of the answer
            if (answer is null)
            {
                return NotFound(new ErrorResponse() {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "không tìm thấy câu trả lời tương ứng"
                });
            }
            //get the question from the answer
            QuestionModel question = await _questionService.GetByIdAsync(answer.QuestionId);

            //check if the question is able to edit
            if (question.IsActive == false)
            {
                return BadRequest(new ErrorResponse() {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Câu hỏi đã nằm trong đề thi và không được chỉnh sửa"
                });
            }
            

            bool isSucess = await _answerService.DeleteAnswerAsync(answerId);
            if (!isSucess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else if (question.QuestionAnswers.Count < 2) 
            {
                return Ok(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "vui lòng tạo thêm câu trả lời"
                });
            }
            return Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Xóa thành công"
            });
        }

        [HttpGet]
        [Route("theory/{questionId:int}/view-detail")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> ViewDetailQuestion([FromRoute] int questionId) 
        {
            var question = await _questionService.GetByIdAsync(questionId);
            if (question is null) 
            {
                return BadRequest(new ErrorResponse() {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Câu hỏi không tồn tại"
                });
            }
            //var answer = await _answerService.GetAllByQuestionId(questionId);
            //if (answer is null) 
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError); 
            //}
            return Ok(new BaseResponse() {
                StatusCode = StatusCodes.Status200OK,
                Data = question
            });
        }

        [HttpGet]
        [Route("theory/question-bank/{licenseId:int}")]
        public async Task<IActionResult> GetQuestionBankWithLicenseId([FromRoute] int licenseId)
        {

            TheoryExamCreateRequirementModel createRule = null!;
            var licenseType = await _licenseTypeService.GetAsync(licenseId);
            if (licenseType.LicenseTypeDesc.Equals("A1"))
            {
                createRule = _theoryExamSettings.CreateRules[0];
            }
            else if (licenseType.LicenseTypeDesc.Equals("A2"))
            {
                createRule = _theoryExamSettings.CreateRules[1];
            }
            else if (licenseType.LicenseTypeDesc.Equals("B1"))
            {
                createRule = _theoryExamSettings.CreateRules[2];
            }
            else if (licenseType.LicenseTypeDesc.Equals("B2"))
            {
                createRule = _theoryExamSettings.CreateRules[3];
            }

            var questions = await _questionService.GetAllByLicenseId(licenseId);
            var questionWithAnswersModel = new List<QuestionWithAnswersModel>();
            if (questions is null || questions.Count() == 0)
            {
                return NotFound(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không tìm thấy câu hỏi theo loại đề này"
                });
            }

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
                questionWithAnswersModel.Add(new QuestionWithAnswersModel
                {
                    question = question,
                    answers = answers
                });
            }


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
                    CreateRule = createRule
                }
            });
        }

        [HttpDelete]
        [Route("theory/{questionId:int}/delete-question")]
        // [Authorize(Roles = "Admin,Staff")]
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
                    Message = "Không tìm thấy câu hỏi"
                });
            }
            // duyet isActive con tai trong de nao khac k
            bool isExisted = await _theoryExamService.IsExamQuestion(question.QuestionId);
            if (isExisted)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Câu hỏi đã nằm trong đề thi và không được chỉnh sửa"
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
        [Authorize(Roles = "Admin,Staff")]
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
            int pageSize = _appSettings.PageSize;
            //paging
            var list = PaginatedList<QuestionWithAnswersModel>.CreateByIEnumerable(questionWithAnswersModel, page, pageSize);

            //not found suitable question with answer
            if (questionWithAnswersModel is null)
            {
                    return BadRequest(new ErrorResponse()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Không tìm thấy câu hỏi"
                    });
            }
            return Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    QuestionWithAnswer = questionWithAnswersModel,
                    TotalQuestion = questionWithAnswersModel.Count(),
                    TotalPage = list.TotalPage,
                    PageIndex = list.PageIndex

                }
            });


        }
        //[HttpGet]
        //[Route("theory/")]


        //cho ngan hang cau hoi
        [HttpGet]
        [Route("theory")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllQuestion() 
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
                
            }
            return Ok(new BaseResponse() { 
                StatusCode = StatusCodes.Status200OK,
                Data = result
            });
        }

        [HttpGet]
        [Route("theory/license-type/{licenseTypeId:int}")]
        //[Authorize(Roles = "Member,Staff,Admin")]
        public async Task<IActionResult> GetAllTheoryByLicenseTypeId([FromRoute] int licenseTypeId) 
        {
            var theoryExams = await _theoryExamService.GetByLicenseTypeIdAsync(licenseTypeId);
            if (theoryExams is null) 
            {
                return NotFound(new ErrorResponse() { 
                    StatusCode = StatusCodes.Status404NotFound,
                    Message ="không có đề cho loại bằng lái này"
                });
            }
            return Ok(new BaseResponse() { 
                StatusCode = StatusCodes.Status200OK,
                Data = theoryExams
            });

        }

    }
}
// answerService AnswerModel -> QuestionID -> QuestionService
// createAsync -> questionModel -> questionId -> foreach List<AnswerModel> list -> each.QuestionId = questionId (controller)

// Repository : Tuong tac truc tiep voi db -> param : Entity, Entity Field
// Service: 