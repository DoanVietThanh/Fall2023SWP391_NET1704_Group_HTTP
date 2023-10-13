using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IAnswerRepository
    {
        public Task<AnswerModel> CreateAsync(QuestionAnswer answer);
        public Task<IEnumerable<AnswerModel>> CreateRangeAsync(IEnumerable<QuestionAnswer> answers);

        public Task<IEnumerable<AnswerModel>> GetAllByQuestionId(int questionId);
        public Task<bool> DeleteAnswerAsync(int answerId);

        public Task<bool> DeleteAnswersByQuestionIdAsync(int quesitonId);

        public Task<AnswerModel> GetByAnswerIdAsync(int answerId);

        public Task<int> GetRightAnswerIdByQuestionId(int questionId);
        Task<AnswerModel> GetByQuestionIdAndAnswerDesc(int questionId, string answerDesc);
        Task<AnswerModel> UpdateAnswerAsync(int answerId, AnswerModel answer);

    }

}
