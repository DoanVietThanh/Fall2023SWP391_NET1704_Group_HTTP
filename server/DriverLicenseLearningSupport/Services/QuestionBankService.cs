using AutoMapper;
using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Services.Impl;

namespace DriverLicenseLearningSupport.Services
{
    public class QuestionBankService : IQuestionBankService
    {
        private readonly DriverLicenseLearningSupportContext _context;
        private readonly IMapper _mapper;

        public QuestionBankService(DriverLicenseLearningSupportContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<IEnumerable<AnswerModel>> GetAnswerByQuestionId(int questionId)
        {
            var allAnswerOfQuestion = _context.
        }

        public Task<AnswerModel> GetRightAnswerForQuestion(int questionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateQuestion(QuestionModel currentQuestion, QuestionModel updatedQuestion)
        {
            throw new NotImplementedException();
        }
    }
}
