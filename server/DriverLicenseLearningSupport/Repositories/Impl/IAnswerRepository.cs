using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IAnswerRepository
    {
        public Task<IEnumerable<AnswerModel>> getAllAnwser();
        public Task<AnswerModel> GetAnswerAsync(int answerId);

        public Task CreateAnswerAsync(QuestionAnswer answer);

        public Task<IEnumerable<AnswerModel>> getAnswserofQuestion(int questionId);
        public Task RemoveAnswerAsync(int questionId);

    }

}
