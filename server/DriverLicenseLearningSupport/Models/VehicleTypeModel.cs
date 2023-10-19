namespace DriverLicenseLearningSupport.Models
{
    public class VehicleTypeModel
    {
        public int VehicleTypeId { get; set; } = 0;
        public int? LicenseTypeId { get; set; } = null!;
        public string VehicleTypeDesc { get; set; } = null!;
        public double? Cost { get; set; } = 0;

        public virtual LicenseTypeModel LicenseType { get; set; } = null!;
    }
}
