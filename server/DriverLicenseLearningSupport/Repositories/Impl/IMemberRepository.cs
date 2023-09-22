using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Filters;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IMemberRepository
    {
        Task<MemberModel> CreateAsync(Member member);
        Task<IEnumerable<MemberModel>> CreateRangeAsync(IEnumerable<Member> members);
        Task<bool> UpdateAsync(Guid id, MemberModel member);
        Task<MemberModel> GetAsync(Guid id);
        Task<IEnumerable<MemberModel>> GetAllAsync();
        Task<IEnumerable<MemberModel>> GetAllAsyncByFilter(MemberFilter filters);
        Task<MemberModel> GetByEmailAsync(string email);
        Task<bool> DeleteAsync(Guid id);
    }
}
