using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IMemberService
    {
        Task<bool> CreateAsync(MemberModel member);
    }
}
