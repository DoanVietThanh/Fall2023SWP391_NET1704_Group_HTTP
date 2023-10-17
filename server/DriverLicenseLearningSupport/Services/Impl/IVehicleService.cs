using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IVehicleService
    {
        Task<VehicleModel> CreateAsync(VehicleModel model);
        Task<IEnumerable<VehicleTypeModel>> GetAllVehicleTypeAynsc();
        Task<VehicleModel> GetVehicleInDateScheduleAsync(DateTime teachingDate, int vehicleTypeId);
        Task<VehicleModel> GetVehicleByVehicleTypeAsync(int vehicleTypeId);
        Task<VehicleTypeModel> GetVehicleTypeByLicenseTypeAsync(int licenseTypeId);
        Task<bool> UpdateActiveStatusAsync(int vehicleId);
    }
}
