using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IExamHistoryService
    {
        Task<ExamHistoryModel> CreateAsync(ExamHistoryModel model);

        Task<ExamHistoryModel> GetHistoryDetailAsync(string MemberId, int TheoryExamId, DateTime joinDate);
        Task<IEnumerable<ExamHistoryModel>> GetAllByMemberIdAsysn(string memberId);
        Task<IEnumerable<ExamHistoryModel>> GetAllExamHistory();
    }
}
