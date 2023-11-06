using Amazon.S3.Model;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IQuestionService
    {
        Task<QuestionModel> CreateAsync(QuestionModel question);

        Task<IEnumerable<QuestionModel>> GetAllAsync();
        Task<IEnumerable<QuestionModel>> GetAllByLicenseId(int licenseId);
        Task<QuestionModel> GetByIdAsync(int questionIs);
        Task<List<QuestionModel>> GetAllInExam(int theoryExamId);
        Task<bool> DeleteQuestionAsync(int questionId);
        Task<QuestionModel> UpdateStatusQuestionAsync(int questionId, bool status);
        Task<QuestionModel> UpdateQuestionAsync(QuestionModel updatedModel, int questionId);
        Task<bool> CheckExistedQuestion(string questionDesc, int lisenceId);
        
    }
}
