using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IRollCallBookRepository
    {
        Task<IEnumerable<RollCallBookModel>> GetAllByMemberIdAsync(Guid memberId);
    }
}
