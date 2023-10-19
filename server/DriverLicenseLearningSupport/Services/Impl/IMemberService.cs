using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Payloads.Filters;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IMemberService
    {
        Task<MemberModel> CreateAsync(MemberModel member);
        Task<LicenseRegisterFormModel> CreateLicenseRegisterFormAsync(LicenseRegisterFormModel licenseRegisterForm, Guid memberId);
        Task<IEnumerable<MemberModel>> CreateRangeAsync(IEnumerable<MemberModel> members);  
        Task<MemberModel> GetAsync(Guid id);
        Task<IEnumerable<MemberModel>> GetAllAsync();
        Task<IEnumerable<MemberModel>> GetAllByFilterAsync(MemberFilter filters);
        Task<MemberModel> GetByEmailAsync (string email);
        Task<MemberModel> GetByLicenseRegisterFormIdAsync(int licenseRegisterFormId);  
        Task<bool> UpdateAsync(Guid id,  MemberModel member);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> HideMemberAsync(Guid id);
    }
}
