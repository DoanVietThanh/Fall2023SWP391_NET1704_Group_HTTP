using DriverLicenseLearningSupport.Payloads.Request;
using DriverLicenseLearningSupport.Repositories.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace DriverLicenseLearningSupport.Controllers
{
    [ApiController]
    public class ExamGradeController : ControllerBase
    {
        private readonly IExamGradeRepository _examGradeRepository;

        public ExamGradeController(IExamGradeRepository examGradeRepository) 
        {
            _examGradeRepository = examGradeRepository;
        }

        [HttpPost]
        [Route("demo")]
        public async Task<IActionResult> CreateGradeForTest([FromForm] SubmitAnswerRequest reqObj) 
        {
            var model = reqObj.ToListExamGradeModel();
            return Ok(reqObj);

        }
    }
}
