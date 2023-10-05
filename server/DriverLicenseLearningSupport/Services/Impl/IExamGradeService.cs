using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IExamGradeService
    {
        Task<ExamGradeModel> CreateAsync(ExamGradeModel model);

        Task<List<ExamGradeModel>> GetAllByTheoryExamIdandEmailAsync(string Email, int TheoryExamId, DateTime StartedDate);
    }
}
