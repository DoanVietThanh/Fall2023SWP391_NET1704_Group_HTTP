using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IMemberRepository
    {
        Task<bool> CreateAsync(Member member);
        Task<MemberModel> FindByIdAsync(Guid id);
        Task<MemberModel> FindByEmailAsync(string email);
    }
}
