using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IQuestionService
    {
        Task<QuestionModel> CreateAsync(QuestionModel question);

        Task<IEnumerable<QuestionModel>> GetAllAsync();

        Task<QuestionModel> GetByIdAsync(int questionIs);
        Task<bool> DeleteQuestionAsync(int questionId);

    }
}
