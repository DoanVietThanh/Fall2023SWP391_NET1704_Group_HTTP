using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IVehicleRepository
    {
        Task<VehicleModel> CreateAsync(Vehicle vehicle);
        Task<VehicleModel> GetByLicenseTypeIdAsync(int licenseTypeId);
        Task<IEnumerable<VehicleTypeModel>> GetAllVehicleTypeAsync();
    }
}
