using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ILicenseRegisterFormRepository
    {
        Task<LicenseRegisterFormModel> CreateAsync(LicenseRegisterForm licenseRegister, Guid memberId); 
        
    }
}
