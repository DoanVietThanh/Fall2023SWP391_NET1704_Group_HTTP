using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IVehicleService
    {
        Task<VehicleModel> CreateAsync(VehicleModel model);
        Task<IEnumerable<VehicleTypeModel>> GetAllVehicleTypeAynsc();
        Task<VehicleModel> GetByLicenseTypeIdAsync(int licenseTypeId);
    }
}
