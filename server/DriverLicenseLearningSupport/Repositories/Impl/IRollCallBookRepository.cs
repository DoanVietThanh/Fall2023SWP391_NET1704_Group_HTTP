using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IRollCallBookRepository
    {
        Task<RollCallBookModel> GetAsync(int id);
        Task<IEnumerable<RollCallBookModel>> GetAllByMemberIdAsync(Guid memberId);
        Task<IEnumerable<RollCallBookModel>> GetAllInActiveRollCallBookAsync();
        Task<bool> UpdateAsync(int rcbId, RollCallBookModel rcbook);
        Task<bool> UpdateInActiveStatusAsync(int rcbId, string cancelMessage);
        Task<bool> ApproveCancelAsync(int rcbId);
        Task<bool> DenyCancelSchedule(int rcbId);
    }
}
