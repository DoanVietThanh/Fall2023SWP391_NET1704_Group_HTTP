using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using Microsoft.EntityFrameworkCore;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IQuestionRepository
    {
        Task<QuestionModel> CreateAsync(Question question);
        Task<IEnumerable<QuestionModel>> GetAllAsync();
        Task<IEnumerable<QuestionModel>> GetAllByLicenseId(int lisenceId);
        Task<bool> DeleteQuestionAsync(int questionId);
        Task<QuestionModel> GetByIdAsync(int questionId);
        Task<QuestionModel> UpdateStatusQuestionAsync(int questionId, bool status);


    }
}
