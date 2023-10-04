using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ITheoryExamRepository
    {
        public Task<TheoryExamModel> CreateAsync(TheoryExam theoryExam);
        Task<bool> AddQuesitonAsync(int theoryExamId, int questionId);

        Task<bool> IsExamQuestion(int questionId);
    }       
}
