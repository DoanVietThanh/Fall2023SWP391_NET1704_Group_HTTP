using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IQuestionRepository
    {
        Task<QuestionModel> CreateAsync(Question question);
        Task<IEnumerable<QuestionModel>> GetAllAsync();
        Task<bool> DeleteQuestionAsync(int questionId);
        Task<QuestionModel> GetByIdAsync(int questionId);

    }
}
