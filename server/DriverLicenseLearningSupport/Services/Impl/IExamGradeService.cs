using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IExamGradeService
    {
        Task<ExamGradeModel> CreateAsync(ExamGradeModel model);
    }
}
