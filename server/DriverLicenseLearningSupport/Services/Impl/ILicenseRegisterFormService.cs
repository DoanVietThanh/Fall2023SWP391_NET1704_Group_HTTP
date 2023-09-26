using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ILicenseRegisterFormService 
    {
        Task<LicenseRegisterFormModel> CreateAsync(LicenseRegisterFormModel model, Guid memberId);
        Task<LicenseRegisterFormModel> GetAsync(int licenseRegisterId);
        Task<bool> ApproveAsync(int licenseRegisterId);
    }
}
