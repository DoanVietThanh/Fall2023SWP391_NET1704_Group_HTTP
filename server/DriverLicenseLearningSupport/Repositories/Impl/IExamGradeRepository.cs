using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IExamGradeRepository
    {
        Task<ExamGradeModel> CreateAsync(ExamGrade entity);
    }
}
