using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IAnswerService
    {
        Task<AnswerModel> CreateAsync(AnswerModel answer);
        Task<IEnumerable<AnswerModel>> CreateRangeAsync(IEnumerable<AnswerModel> answers);

        Task<IEnumerable<AnswerModel>> GetAllByQuestionId(int questionId);
        Task<bool> DeleteAnswerAsync(int answerId);

        Task<bool> DeleteAnswersByQuestionIdAsync(int quesitonId);
        Task<AnswerModel> GetByAnswerIdAsync(int answerId);

        public Task<int> GetRightAnswerIdByQuestionId(int questionId);

        Task<AnswerModel> GetByQuestionIdAndAnswerDesc(int questionId, string answerDesc);
    }
}
