using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.impl
{
    public interface ILicenseTypeService
    {
        Task<IEnumerable<LicenseTypeModel>> GetAllAsync();
    }
}
