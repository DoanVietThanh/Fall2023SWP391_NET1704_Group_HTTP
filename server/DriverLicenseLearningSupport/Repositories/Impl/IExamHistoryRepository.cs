using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IExamHistoryRepository
    {
        Task<ExamHistoryModel> CreateAsync(ExamHistory examHistory);

        Task<ExamHistoryModel> GetHistoryDetailAsync(string MemberId, int TheoryExamId, DateTime joinDate);

        Task<IEnumerable<ExamHistoryModel>> GetAllByMemberIdAsysn(string memberId);
    }
}
