using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IRollCallBookService
    {
        Task<RollCallBookModel> GetAsync(int id);
        Task<IEnumerable<RollCallBookModel>> GetAllByMemberIdAsync(Guid memberId);
        Task<bool> UpdateAsync(int rcbId, RollCallBookModel rcbook);
    }
}
