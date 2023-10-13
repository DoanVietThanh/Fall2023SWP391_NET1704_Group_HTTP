using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Utils;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class VehicleAddRequest
    {
        [Required(ErrorMessage = "Vehicle name is required")]
        public string VehicleName { get; set; }

        [Required(ErrorMessage = "Vehicle license plate is required")]
        public string VehicleLicensePlate { get; set; }

        [Required(ErrorMessage = "Registration date is required")]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "Vehicle Type is required")]
        public int VehicleTypeId { get; set; }

        public IFormFile VehicleImage { get; set; }
    }

    public static class VehicleAddRequestExtension
    {
        public static VehicleModel ToVehicleModel(this VehicleAddRequest reqObj, string dateFormat)
        {
            return new VehicleModel { 
                VehicleName = reqObj.VehicleName,
                VehicleLicensePlate = reqObj.VehicleLicensePlate,
                RegisterDate = reqObj.RegistrationDate,
                VehicleTypeId = reqObj.VehicleTypeId
            };
        }
    }
}
