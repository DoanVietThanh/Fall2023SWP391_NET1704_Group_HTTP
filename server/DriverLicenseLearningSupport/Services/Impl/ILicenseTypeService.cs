using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface ILicenseTypeService
    {
        Task<IEnumerable<LicenseTypeModel>> GetAllAsync();
        Task<LicenseTypeModel> GetAsync(int id);
        Task<LicenseTypeModel> GetByDescAsync(string licenseTypeDesc);

    }
}
