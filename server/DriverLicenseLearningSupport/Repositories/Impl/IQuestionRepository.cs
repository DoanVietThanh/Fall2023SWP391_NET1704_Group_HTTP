using DriverLicenseLearningSupport.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IQuestionRepository
    {
        public Task<IEnumerable<QuestionModel>> GetAllQuesAsync();
        public Task<QuestionModel> GetQuesAsync(int questionId);

        public Task<int> CreateQuesAsync(QuestionModel question);

        public Task RemoveQuesAsync(int questionId);
    }
}
