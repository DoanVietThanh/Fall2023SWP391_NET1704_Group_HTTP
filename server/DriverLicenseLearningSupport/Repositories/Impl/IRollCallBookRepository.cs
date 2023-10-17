using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IRollCallBookRepository
    {
        Task<RollCallBookModel> GetAsync(int id);
        Task<IEnumerable<RollCallBookModel>> GetAllByMemberIdAsync(Guid memberId);
        Task<bool> UpdateAsync(int rcbId, RollCallBookModel rcbook);
    }
}
