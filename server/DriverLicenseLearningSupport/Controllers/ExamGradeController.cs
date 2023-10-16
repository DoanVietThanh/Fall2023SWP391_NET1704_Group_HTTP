using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Models.Config;
using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Payloads.Response;
using DriverLicenseLearningSupport.Repositories.Impl;
using DriverLicenseLearningSupport.Services;
using DriverLicenseLearningSupport.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto;
using System.Globalization;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class ExamGradeController : ControllerBase
    {
        private readonly IExamGradeService _examGradeService;
        private readonly IAnswerService _answerService;
        private readonly ITheoryExamService _theoryExamService;
        private readonly IExamHistoryService _examHistoryService;
        private readonly IQuestionService _questionService;
        private readonly IMemberService _memberService;
        private readonly AppSettings _appSettings;

        public ExamGradeController(IExamGradeService examGradeService, IAnswerService answerService
            , ITheoryExamService theoryExamService, IExamHistoryService examHistoryService
            , IQuestionService questionService, IMemberService memberService, IOptionsMonitor<AppSettings> monitor)
        {
            _examGradeService = examGradeService;
            _answerService = answerService;
            _theoryExamService = theoryExamService;
            _examHistoryService = examHistoryService;
            _questionService = questionService;
            _memberService = memberService;
            _appSettings = monitor.CurrentValue;
        }

        [HttpPost]
        [Route("theory/submit")]
        //[Authorize(Roles = ("Member"))]
        public async Task<IActionResult> CreateGradeForTest([FromBody] SubmitAnswerRequest reqObj)
        {
            // tao id cho cau hoi dung, so cau dung, tong so cau hoi
            int theRightAnswerId = 0;
            int countRightAnswers = 0;
            int totalQuesiton = 0;
            bool isWrongParalysisQuesion = false;
            bool isPassed = true;
            DateTime startedDate = DateTime.ParseExact(reqObj.StartedDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            var models = reqObj.ToListExamGradeModel();

            //lay memberid neu co
            var member = await _memberService.GetByEmailAsync(reqObj.Email);


            //lay de thi
            var theoryExam = await _theoryExamService.GetByIdAsync(models[0].TheoryExamId);
            //lay tong so cau hoi, so cau dung yeu cau
            totalQuesiton = Convert.ToInt32(theoryExam.TotalQuestion);
            int requiredRightAnswer = Convert.ToInt32(theoryExam.TotalAnswerRequired);

            List<ExamGradeModel> listResult = new List<ExamGradeModel>();
            foreach (ExamGradeModel examGradeModel in models)
            {
                // get selected answer model
                //var reqModel = reqObj.SelectedAnswers.Where(x => x.QuestionId == examGradeModel.QuestionId).FirstOrDefault(); 
                //b2.so sánh với id của selected answerid -> gắn model -> có được answer Detail
                //b3. từ answer detail lấy được 
                
                ////var selectAnswerModel = await _answerService.GetByAnswerIdAsync(reqModel.SelectedAnswerId);

                //// set select answer id 
                //examGradeModel.SelectedAnswerId = selectAnswerModel.QuestionAnswerId;

                // get question by id
                QuestionModel question = await _questionService.GetByIdAsync(examGradeModel.QuestionId);

                //get all answer of the question 
                var answers = await _answerService.GetAllByQuestionId(examGradeModel.QuestionId);

                var theRightAnswerModel = answers.Where(x => x.IsTrue == true)
                    .Select(x => new AnswerModel() 
                    {
                        QuestionAnswerId = x.QuestionAnswerId,
                        Answer = x.Answer
                    }
                    ).FirstOrDefault();
                var selectedAnswerModel = answers.Where(x => x.QuestionAnswerId == examGradeModel.SelectedAnswerId)
                    .FirstOrDefault();
                if (selectedAnswerModel is null) 
                {
                    selectedAnswerModel = new AnswerModel()
                    {
                        QuestionAnswerId = 0
                    };
                }
                    
                

                //var theRightAnswerId = await _answerService.GetRightAnswerByDesc(reqObj);

                // set member id
                examGradeModel.MemberId = member.MemberId;
                if (theRightAnswerModel.QuestionAnswerId == examGradeModel.SelectedAnswerId)
                {
                    examGradeModel.Point = 1;
                    countRightAnswers++;
                }
                else if (question.IsParalysis is true)
                {
                    isWrongParalysisQuesion = true;
                    examGradeModel.Point = 0;
                }
                else
                {
                    examGradeModel.Point = 0;
                }


                var date = startedDate.ToString(_appSettings.DateTimeFormat);
                startedDate = DateTime.ParseExact(date, _appSettings.DateTimeFormat, CultureInfo.InvariantCulture);
                examGradeModel.StartedDate = startedDate;

                //right answer với id là 0,1,2,3 -> lấy nội dung và questionid để gán lại id dưới db
                var answer = await _answerService.GetByQuestionIdAndAnswerDesc(examGradeModel.QuestionId,theRightAnswerModel.Answer);
                // gán lại vào db, bảng examGrade selectedanswerId tương ứng ở dưới db
                examGradeModel.SelectedAnswerId = answer.QuestionAnswerId; 

                var createdExamGradeModel = await _examGradeService.CreateAsync(examGradeModel);

                //var startedDateFormat = Convert.ToDateTime(createdExamGradeModel.StartedDate).ToString("dd-MM-yyyy HH:mm:ss");

                createdExamGradeModel.StartedDate = startedDate;

                //createdExamGradeModel.MemberId = member.MemberId;

                listResult.Add(createdExamGradeModel);
            }
            if (listResult is null)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = "lỗi tải"
                });
            }
            if (isWrongParalysisQuesion is true || countRightAnswers < requiredRightAnswer)
            {
                isPassed = false;
            }

            //sinh history
            var HistoryModel = new ExamHistoryModel()
            {
                MemberId = member.MemberId,
                TheoryExamId = reqObj.TheoryExamId,
                TotalQuestion = totalQuesiton,
                TotalRightAnswer = countRightAnswers,
                IsPassed = isPassed,
                WrongParalysisQuestion = isWrongParalysisQuesion,
                TotalTime = reqObj.TotalTime,
                Date = startedDate
            };
            var createdHistoryModel = await _examHistoryService.CreateAsync(HistoryModel);
            if (createdHistoryModel is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            /*
            return new ObjectResult(new
            {
                ListGrade = listResult,
                TotalQuestion = totalQuesiton,
                TotalRightAnswer = countRightAnswers,
                IsPassed = isPassed,
                HistoryModel = createdHistoryModel

                }
            })
            { StatusCode = StatusCodes.Status201Created };*/

            return Ok(new BaseResponse { 
                StatusCode = StatusCodes.Status200OK,
                Message = "Submit theory test successfully!"
            });

            //return Ok(new BaseResponse() { 
            //    StatusCode = StatusCodes.Status200OK,
            //    Data = new
            //    {
            //        ListGrade = listResult,
            //        TotalQuestion = totalQuesiton,
            //        TotalRightAnswer = countRightAnswers

            //    },

            //    Message ="Thành công"
            //});

        }

        //api get all history của member đó -> có nút review -> theory/review (review Detail)
        [HttpGet]
        [Route("theory/history/{id:Guid}")]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> GetAllHistoryOfMember([FromRoute] Guid id)
        {
            MemberModel memberModel = await _memberService.GetAsync(id);
            if (memberModel is null)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Lỗi xuất rì viu"
                });
            }
            var Histories = await _examHistoryService.GetAllByMemberIdAsysn(memberModel.MemberId);

            if (Histories is null)
            {
                return NotFound(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "chưa có lịch sử thi"
                });
            }
            return Ok(new BaseResponse()
            {
                StatusCode = StatusCodes.Status200OK,
                Data = Histories
            });
        }


        //api cho việc đã ấn review
        [HttpPost]
        [Route("theory/review")]
        //[Authorize(Roles = "Member")]
        public async Task<IActionResult> ReviewDetailedMockTest([FromBody] ReviewExamRequest reqObj)
        {
            MemberModel memberModel = await _memberService.GetByEmailAsync(reqObj.Email);
            if (memberModel is null)
            {
                return BadRequest(new ErrorResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Lỗi rì viu"
                });
            }
            //var startedDate = joinDate.ToString(_appSettings.DateFormat);
            List<ExamGradeModel> examGrades = await _examGradeService.GetAllByTheoryExamIdandEmailAsync(reqObj.Email
                , reqObj.MockTestId, reqObj.JoinDate);
            //lay toan bo cau hoi va dap an trong de thi


            foreach (ExamGradeModel eg in examGrades)
            {
                var selectedAnswer = await _answerService.GetByAnswerIdAsync(eg.SelectedAnswerId);
                QuestionModel question = await _questionService.GetByIdAsync(eg.QuestionId);
                IEnumerable<AnswerModel> answers = await _answerService.GetAllByQuestionId(eg.QuestionId);
                foreach (AnswerModel answer in answers) 
                {
                    if (answer.Answer.Equals(selectedAnswer.Answer)) 
                    {
                        eg.SelectedAnswerId = answer.QuestionAnswerId; 
                        break;
                    }
                }
                question.QuestionAnswers = answers.ToList();
                eg.Question = question;
            }



            ExamHistoryModel history = await _examHistoryService.GetHistoryDetailAsync(memberModel.MemberId
                , reqObj.MockTestId, reqObj.JoinDate);
            {
                if (history is null)
                {
                    return NotFound(new ErrorResponse()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "không thấy lịch sử thi"
                    });
                }

                return Ok(new BaseResponse()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Data = new
                    {
                        ExamResult = examGrades,
                        History = history
                    }
                });
            }

            //api filter (theo giờ phút và ngày thi) , TheoryExamId ,
            //, int Hours, int Minutes
            // 2023-10-05T23:15:0
            //TimeSpan ts = new TimeSpan(Hours ,Minutes, 0); // 
            // DateTime dt = get from DB -> Date, Time
            // dt.ToSring(_appSetign.DateFormat)
            // dt.toString("HH:mm:ss").equal(ts.toString())
        }
    }
}
