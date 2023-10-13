using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IRollCallBookService
    {
        Task<IEnumerable<RollCallBookModel>> GetAllByMemberIdAsync(Guid memberId);
    }
}
