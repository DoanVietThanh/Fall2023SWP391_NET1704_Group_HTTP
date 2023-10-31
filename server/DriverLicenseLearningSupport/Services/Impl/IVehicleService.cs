using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IVehicleService
    {
        Task<VehicleModel> CreateAsync(VehicleModel model);
        Task<VehicleModel> GetAsync(int vehicleId);
        Task<IEnumerable<VehicleModel>> GetAllAsync();
        Task<IEnumerable<VehicleTypeModel>> GetAllVehicleTypeAynsc();
        Task<IEnumerable<VehicleModel>> GetAllActiveVehicleByType(int vehicleTypeId);
        Task<IEnumerable<VehicleModel>> GetAllInActiveVehicleByType(int vehicleTypeId);
        Task<VehicleModel> GetVehicleInDateScheduleAsync(DateTime teachingDate, int vehicleTypeId);
        Task<VehicleModel> GetVehicleByVehicleTypeAsync(int vehicleTypeId);
        Task<VehicleTypeModel> GetVehicleTypeByLicenseTypeAsync(int licenseTypeId);
        Task<bool> UpdateActiveStatusAsync(int vehicleId);
        Task<bool> UpdateAsync(int vehicleId, VehicleModel vehicle);
        Task<bool> DeleteAsync(int vehicleId);
    }
}
