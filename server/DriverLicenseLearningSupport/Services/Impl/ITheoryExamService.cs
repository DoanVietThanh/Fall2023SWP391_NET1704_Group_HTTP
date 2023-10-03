using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ITheoryExamService
    {
        Task<TheoryExamModel> CreateAsync(TheoryExamModel theoryExam);

        Task<bool> AddQuestionAsync(int theoryExamId, int quesitonId);

        Task<bool> IsExamQuestion(int questionId);

        
    }
}
