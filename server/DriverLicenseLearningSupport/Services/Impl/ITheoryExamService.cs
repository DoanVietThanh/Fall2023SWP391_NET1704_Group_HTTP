using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ITheoryExamService
    {
        Task<TheoryExamModel> CreateAsync(TheoryExamModel theoryExam);

        Task<bool> AddQuestionAsync(int theoryExamId, int quesitonId);

        Task<bool> IsExamQuestion(int questionId);

        Task<IEnumerable<TheoryExamModel>> GetAllAsync();

        Task<bool> HasHistory(int id);

        Task<TheoryExamModel> GetByIdAsync(int id);
        Task<IEnumerable<TheoryExamModel>> GetByLicenseTypeIdAsync(int licenseTypeId);
        Task<bool> RemoveTheoryExam(int id);
        Task<IEnumerable<TheoryExamModel>> GetAllMockTest();
    }
}
