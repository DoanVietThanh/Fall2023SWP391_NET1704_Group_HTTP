using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Filters;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IMemberService
    {
        Task<MemberModel> CreateAsync(MemberModel member);
        Task<IEnumerable<MemberModel>> CreateRangeAsync(IEnumerable<MemberModel> members);  
        Task<bool> UpdateAsync(Guid id,  MemberModel member);
        Task<MemberModel> GetAsync(Guid id);
        Task<IEnumerable<MemberModel>> GetAllAsync();
        Task<IEnumerable<MemberModel>> GetAllByFilterAsync(MemberFilter filters);
        Task<MemberModel> GetByEmailAsync (string email);
        Task<bool> DeleteAsync(Guid id);
    }
}
