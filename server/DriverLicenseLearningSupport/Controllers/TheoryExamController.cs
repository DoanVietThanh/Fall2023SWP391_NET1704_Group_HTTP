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
using System.Globalization;

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
        private readonly TheoryExamSettings _theoryExamSettings;

        public TheoryExamController(ITheoryExamService theoryExamService, IAnswerService answerService,
            ILicenseTypeService licenseTypeService, IQuestionService questionService,
            IMemoryCache memoryCache, IOptionsMonitor<AppSettings> monitor,
            IOptionsMonitor<TheoryExamSettings> monitor1)
        {
            _theoryExamService = theoryExamService;
            _licenseTypeService = licenseTypeService;
            _questionService = questionService;
            _answerService = answerService;
            _memoryCache = memoryCache;
            _appSettings = monitor.CurrentValue;
            _theoryExamSettings = monitor1.CurrentValue;
        }


        [HttpGet]
        [Route("theory-exam/add-question")]
        //[Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddQuestionToExam()
        {
            var theoryCreateRules = _theoryExamSettings.CreateRules;
            if (theoryCreateRules.Count() > 0)
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = theoryCreateRules
                });
            }

            return BadRequest(new BaseResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Vui lòng thêm các điều kiện để tạo đề thi"
            });
        }


        [HttpPost]
        [Route("theory-exam/add-question")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddQuestionToExam([FromForm] TheoryAddRequest reqObj)
        {
            int currentLicenceId = 0;
            DateTime startDate;
            TimeSpan startTime;

            

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
                        Message = "Lỗi tạo đề thi"
                    });
                }
                //change status
                question = await _questionService.UpdateStatusQuestionAsync(questionId, false);
                //question.TheoryExamIds.Add(mocktest.i)
                //add question theoryTest
                listQuestions.Add(question);
            }

            TheoryExamModel mocktest = new TheoryExamModel();

            mocktest.TotalTime = reqObj.TotalTime;
            mocktest.TotalQuestion = reqObj.TotalQuestion;
            mocktest.TotalAnswerRequired = reqObj.TotalAnswerRequired;
            if (mocktest.TotalQuestion > reqObj.QuestionIds.Count())
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Số câu hiện tại là "+ reqObj.QuestionIds.Count()+ "/"+reqObj.TotalQuestion+". Vui lòng thêm câu hỏi trước khi tạo đề!" 
                });
            }if (mocktest.TotalQuestion < reqObj.QuestionIds.Count())
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Số câu hiện tại là " + reqObj.QuestionIds.Count() + "/" + reqObj.TotalQuestion + ". Vui lòng xóa bớt câu hỏi trước khi tạo đề! "
                });
            }
            //set LicenseType to mock test
            mocktest.LicenseTypeId = listQuestions[0].LicenseTypeId;
            //set list questions to the mock test
            mocktest.Questions = listQuestions;
            mocktest.IsMockExam = reqObj.IsMockTest;

            if (reqObj.IsMockTest == true)
            {
                string stringDateNow = DateTime.Now.ToString("yyyy/MM/dd");
                string stringTimeSpan = DateTime.Now.ToString("HH:mm:ss");
                DateTime dateNow = DateTime.ParseExact(stringDateNow, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                TimeSpan timeSpanNow = TimeSpan.Parse(stringTimeSpan);
                startDate = DateTime.ParseExact(reqObj.StartDate, _appSettings.DateFormat, CultureInfo.InvariantCulture);
                startTime = TimeSpan.Parse(reqObj.Hour.ToString()+":"+reqObj.Minute.ToString());   
                if (startDate < dateNow || (startDate == dateNow && startTime < timeSpanNow)) 
                {
                    return BadRequest(new ErrorResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Không tạo thời gian trước thời điểm hiện tại"
                    });
                }
                startTime = new TimeSpan(reqObj.Hour, reqObj.Minute, 0);
                mocktest.StartDate = startDate;
                mocktest.StartTime = startTime;

            }

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

        // thoery-exam/license-type/id -> list thoery exam id
        [HttpGet]

        [HttpGet]
        [Route("theory-exam/{theoryExamId:int}")]
        public async Task<IActionResult> GetQuestionByTheoryId([FromRoute] int theoryExamId)
        {
            var theoryExam = await _theoryExamService.GetByIdAsync(theoryExamId);
            if (theoryExam is null)
            {
                return NotFound(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "không tìm thấy đề"
                });
            }

            return Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = theoryExam
            });
        }

        [HttpGet]
        [Route("theory-exam/licenseID/{licenseId:int}")]
        public async Task<IActionResult> GetTheoryExamByLicenseId([FromRoute] int licenseId)
        {
            var theoryExams = await _theoryExamService.GetByLicenseTypeIdAsync(licenseId);
            if (theoryExams is null)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Chưa có bộ đề nào"
                });
            }
            else
            {
                return Ok(new BaseResponse
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Tải thành công",
                    Data = theoryExams
                });
            }
        }

        [HttpGet]
        [Route("/mocktest")]
        public async Task<IActionResult> GetAllMockTestAsync() 
        {
            var mocktests = await _theoryExamService.GetAllMockTest();
            if (mocktests is null)
            {
                return NotFound(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Không có đề thi mẫu nào"
                });
            }
            else 
            {
                return Ok(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Tải đề thi mẫu thành công",
                    Data = mocktests
                }) ;
            }
        }   

        
        [HttpDelete]
        [Route("theory-exam/{theoryID:int}")]
        public async Task<IActionResult> DeleteTheoryExam([FromRoute] int theoryID)
        {
            bool hasHistory = await _theoryExamService.HasHistory(theoryID);
            if (hasHistory)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Đề thi đã được làm, không thể xóa"
                });

            }
            bool isSuccess = await _theoryExamService.RemoveTheoryExam(theoryID);
            if (!isSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            else
            {
                return Ok(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status204NoContent,
                    Message = "Xóa đề thành công"
                });
            }
        }
       
    }
}
