using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class VehicleUpdateRequest
    {
        [Required(ErrorMessage = "Image is required")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Vehicle type is required")]
        public int VehicleTypeId { get; set; }


        [Required(ErrorMessage = "Vehicle name is required")]
        public string VehicleName { get; set; }

        [Required(ErrorMessage = "Vehicle name is required")]
        public string VehicleLicensePlate { get; set; }

        [Required(ErrorMessage = "Register date is required")]
        public string RegisterDate { get; set; }
    }

    public static class VehicleUpdateRequestExtension 
    {
        public static VehicleModel ToVehicleModel(this VehicleUpdateRequest reqObj,
            string dateFormat) 
        {
            return new VehicleModel
            {
                VehicleTypeId = reqObj.VehicleTypeId,
                VehicleName = reqObj.VehicleName,
                VehicleLicensePlate = reqObj.VehicleLicensePlate,
                RegisterDate = DateTime.ParseExact(reqObj.RegisterDate,
                    dateFormat, CultureInfo.InvariantCulture)
            }; 
        }
    }
}
