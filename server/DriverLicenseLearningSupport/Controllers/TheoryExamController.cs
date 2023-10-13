using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using DriverLicenseLearningSupport.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using DriverLicenseLearningSupport.Payloads.Request;
using System.Reflection.Metadata.Ecma335;

namespace DriverLicenseLearningSupport.Controllers
{
    public class TheoryExamController : ControllerBase
    {
        private readonly ITheoryExamService _theoryExamService;
        private readonly ILicenseTypeService _licenseTypeService;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly IMemoryCache _memoryCache;
        private readonly AppSettings _appSettings;

        public TheoryExamController(ITheoryExamService theoryExamService, IAnswerService answerService,
            ILicenseTypeService licenseTypeService, IQuestionService questionService,
            IMemoryCache memoryCache, IOptionsMonitor<AppSettings> monitor)
        {
            _theoryExamService = theoryExamService;
            _licenseTypeService = licenseTypeService;
            _questionService = questionService;
            _answerService = answerService;
            _memoryCache = memoryCache;
            _appSettings = monitor.CurrentValue;
        }
        
        [HttpGet]
        [Route("theory-exam/add-question")]
        public async Task<IActionResult> LicenseFormRegister()
        {
            var licenseTypes = await _licenseTypeService.GetAllAsync();

            if (licenseTypes is null) return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Something went wrong"
            });

            List<TheoryAddFormatModel> formats = new List<TheoryAddFormatModel>();
            //Format Mocktest a1
            var licenseTypeA1 = await _licenseTypeService.GetByDescAsync("A1");
            formats.Add(new TheoryAddFormatModel()
            {
                Licensetype = licenseTypeA1.LicenseTypeDesc,
                TotalQuestion = 3,
                TotalAnswerRequired = 21,
                TotalTime = 15
            });
            //Format MockTest a2
            var licenseTypeA2 = await _licenseTypeService.GetByDescAsync("A2");
            formats.Add(new TheoryAddFormatModel()
            {
                Licensetype = licenseTypeA2.LicenseTypeDesc,
                TotalQuestion = 25,
                TotalAnswerRequired = 23,
                TotalTime = 15
            });
            var licenseTypeB1 = await _licenseTypeService.GetByDescAsync("B1");
            formats.Add(new TheoryAddFormatModel()
            {
                Licensetype = licenseTypeB1.LicenseTypeDesc,
                TotalQuestion = 30,
                TotalAnswerRequired = 27,
                TotalTime = 20
            });
            var licenseTypeB2 = await _licenseTypeService.GetByDescAsync("B2");
            formats.Add(new TheoryAddFormatModel()
            {
                Licensetype = licenseTypeB2.LicenseTypeDesc,
                TotalQuestion = 35,
                TotalAnswerRequired = 32,
                TotalTime = 22
            });

            return Ok(new BaseResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = new
                {
                    LicenseTypes = licenseTypes,
                    TheoryFormats = formats
                }
            });
        }

        [HttpGet]
        [Route("theory-exam/question-bank/{licenseId:int}/{page:int}")]
        public async Task<IActionResult> GetQuestionBankWithLicenseId([FromRoute] int licenseId
            , [FromRoute] int page = 1)
        {
            //memory caching
            if (!_memoryCache.TryGetValue(_appSettings.TheoryCacheKey,
                out IEnumerable<QuestionWithAnswersModel> questionWithAnswersModel))
            {
                List<QuestionWithAnswersModel> result = new List<QuestionWithAnswersModel>();
                //get all questions
                var questions = await _questionService.GetAllByLicenseId(licenseId);

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
                }
            });
        }
        
        [HttpPost]
        [Route("theory-exam/add-question")]
        [Authorize (Roles = "Staff")]
        public async Task<IActionResult> AddQuestionToExam([FromForm] TheoryAddRequest reqObj)
        {
            int currentLicenceId = 0;
            List<QuestionModel> listQuestions = new List<QuestionModel>();

            foreach (int questionId in reqObj.QuestionIds)
            {
                //get question
                QuestionModel question = await _questionService.GetByIdAsync(questionId);
                //get the licenseId of the 1st question to compare
                if (currentLicenceId == 0) { currentLicenceId = question.LicenseTypeId; }
                //compareto others quesiton
                if (question.LicenseTypeId != currentLicenceId)
                {
                    return BadRequest(new ErrorResponse()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Error to create one theory mock test"
                    });
                }
                //change status
                question = await _questionService.UpdateStatusQuestionAsync(questionId, false);
                //question.TheoryExamIds.Add(mocktest.i)
                //add question theoryTest
                listQuestions.Add(question);
            }

            TheoryExamModel mocktest = new TheoryExamModel();
            //switch (currentLicenceId)
            //{
            //    case 1:
            //        mocktest.TotalQuestion = 3;
            //        mocktest.TotalAnswerRequired = 21;
            //        mocktest.TotalTime = 15;
            //        break;
            //    case 2:
            //        mocktest.TotalQuestion = 25;
            //        mocktest.TotalAnswerRequired = 23;
            //        mocktest.TotalTime = 15;
            //        break;
            //    case 3:
            //        mocktest.TotalQuestion = 30;
            //        mocktest.TotalAnswerRequired = 27;
            //        mocktest.TotalTime = 20;
            //        break;
            //    case 4:
            //        mocktest.TotalQuestion = 35;
            //        mocktest.TotalAnswerRequired = 32;
            //        mocktest.TotalTime = 22;
            //        break;

            //}
            mocktest.TotalTime = reqObj.TotalTime;
            mocktest.TotalQuestion = reqObj.TotalQuestion;
            mocktest.TotalAnswerRequired = reqObj.TotalAnswerRequired;
            if (mocktest.TotalQuestion != reqObj.QuestionIds.Count())
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Sai số lượng câu cần cho đề thi"
                });
            }
            //set LicenseType to mock test
            mocktest.LicenseTypeId = listQuestions[0].LicenseTypeId;
            //set list questions to the mock test
            mocktest.Questions = listQuestions;

            var result = await _theoryExamService.CreateAsync(mocktest);
            //set mock 
            return Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = result,
                Message = "Tạo đề thành công"
            });
        }

        //[HttpGet]
        //[Route("theory-exam/{page:int}")]
        ////[Authorize(Roles = "Admin,Staff")]
        //public async Task<IActionResult> GetAllTheoryExam([FromRoute] int page = 1)
        //{
        //    //memory caching
        //    if (!_memoryCache.TryGetValue(_appSettings.TheoryCacheKey,
        //        out IEnumerable<TheoryExamModel> theoryexams))
        //    {
        //        theoryexams = await _theoryExamService.GetAllAsync();
        //        // cache options
        //        var cacheEntryOptions = new MemoryCacheEntryOptions()
        //            // none access exceeds 45s <- remove cache
        //            .SetSlidingExpiration(TimeSpan.FromSeconds(45))
        //            // after 10m from first access <- remove cache
        //            .SetAbsoluteExpiration(TimeSpan.FromSeconds(600))
        //            // cache priority
        //            .SetPriority(CacheItemPriority.Normal);
        //        // set cache
        //        _memoryCache.Set(_appSettings.MembersCacheKey, theoryexams, cacheEntryOptions);
        //    }
        //    else
        //    {
        //        theoryexams = (IEnumerable<TheoryExamModel>)_memoryCache.Get(_appSettings.TheoryCacheKey);
        //    }
        //    //page size
        //    int pageSize = _appSettings.TheoryPageSize;
        //    //paging
        //    var list = PaginatedList<TheoryExamModel>.CreateByIEnumerable(theoryexams, page, pageSize);


        //    if (theoryexams is null)
        //    {
        //        return BadRequest(new ErrorResponse()
        //        {
        //            StatusCode = StatusCodes.Status400BadRequest,
        //            Message = "không có bất cứ đề nào"
        //        });
        //    }
        //    return Ok(new BaseResponse()
        //    {
        //        StatusCode = StatusCodes.Status200OK,
        //        Data = new
        //        {
        //            Exams = theoryexams,
        //            TotalExams = theoryexams.Count(),
        //            TotalPage = list.TotalPage,
        //            PageIndex = list.PageIndex
        //        }
        //    }); ;
        //}

        [HttpGet]
        [Route("theory-exam/{theoryExamId:int}")]
        public async Task<IActionResult> GetQuestionByTheoryId([FromRoute] int theoryExamId) 
        {
            var theoryExam = await _theoryExamService.GetByIdAsync(theoryExamId);
            if (theoryExam is null) 
            {
                return NotFound(new ErrorResponse() { 
                    StatusCode= StatusCodes.Status404NotFound,
                    Message ="không tìm thấy đề"
                });
            }
            return Ok(new BaseResponse() {
                StatusCode = StatusCodes.Status200OK,
                Data = theoryExam
            });
        }


    }

}
