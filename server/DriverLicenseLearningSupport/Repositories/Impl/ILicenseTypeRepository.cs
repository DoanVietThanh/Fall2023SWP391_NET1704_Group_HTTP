using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ILicenseTypeRepository
    {
        Task<IEnumerable<LicenseTypeModel>> GetAllAsync();
        Task<LicenseTypeModel> GetAsync(int id);
        Task<LicenseTypeModel> GetByDescAsync(string licenseTypeDesc);

    }
}
