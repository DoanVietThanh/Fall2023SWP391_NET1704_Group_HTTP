using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class VehicleModel
    {
        public int VehicleId { get; set; }
        public string VehicleName { get; set; } = null!;
        public string? VehicleImage { get; set; } = null!;
        public string VehicleLicensePlate { get; set; } = null!;
        public DateTime? RegisterDate { get; set; }
        public int? VehicleTypeId { get; set; } = null!;

        public virtual VehicleTypeModel VehicleType { get; set; } = null!;
    }
}
