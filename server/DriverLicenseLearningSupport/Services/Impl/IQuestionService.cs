using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IQuestionService
    {
        Task<QuestionModel> CreateAsync(QuestionModel question);
        
    }
}
