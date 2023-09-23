using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ILicenseRegisterFormService 
    {
        Task<LicenseRegisterFormModel> CreateAsync(LicenseRegisterFormModel model, Guid memberId);
    }
}
