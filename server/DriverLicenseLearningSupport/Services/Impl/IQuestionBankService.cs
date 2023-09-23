using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IQuestionBankService
    {
        public Task<IEnumerable<AnswerModel>> GetAnswerByQuestionId(int questionId);

        public Task<AnswerModel> GetRightAnswerForQuestion(int questionId);

        public Task<bool> UpdateQuestion(QuestionModel currentQuestion, QuestionModel updatedQuestion);
        
    }
}
