using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IVehicleRepository
    {
        Task<VehicleModel> CreateAsync(Vehicle vehicle);
        Task<VehicleModel> GetVehicleByVehicleTypeAsync(int vehicleTypeId);
        Task<VehicleTypeModel> GetVehicleTypeByLicenseTypeAsync(int licenseTypeId);
        Task<IEnumerable<VehicleTypeModel>> GetAllVehicleTypeAsync();
        Task<IEnumerable<VehicleModel>> GetAllByVehicleTypeIdAsync(int vehicleTypeId);
        Task<bool> UpdateActiveStatusAsync(int vehicleId);
    }
}
