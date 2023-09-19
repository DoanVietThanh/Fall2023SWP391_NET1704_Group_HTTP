using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface ILicenseTypeRepository
    {
        Task<IEnumerable<LicenseTypeModel>> FindAllAsync();
        Task<LicenseTypeModel> FindByIdAsync(int id);
    }
}
