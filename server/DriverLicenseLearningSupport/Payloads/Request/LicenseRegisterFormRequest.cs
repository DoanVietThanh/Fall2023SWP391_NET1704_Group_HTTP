using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Services.Impl;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class LicenseRegisterFormRequest
    {
        //[Required(ErrorMessage = "Image is required")]
        public IFormFile? Image { get; set; } = null!;

        //[Required(ErrorMessage = "Identity Image is required")]
        public IFormFile? IdentityCardImage { get; set; } = null!;
        //[Required(ErrorMessage = "Health certification Image is required")]
        public IFormFile? HealthCertificationImage { get; set; } = null!;

        [Required(ErrorMessage = "Member Id is required")]
        public Guid MemberId { get; set; }
        [Required(ErrorMessage = "License Type Id is required")]
        public int LicenseTypeId { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }
        
        [Required(ErrorMessage = "Permanent Address is required")]
        public string PermanentAddress { get; set; }
       
        [Required(ErrorMessage = "Identity Number is required")]
        public string IdentityNumber { get; set; }

        [Required(ErrorMessage = "ID card issued date is required")]
        public string IdentityCardIssuedDate { get; set; }
        [Required(ErrorMessage = "ID card issued by is required")]
        public string IdentityCardIssuedBy { get; set; }
        [Required(ErrorMessage = "License type issued date is required")]
        public string LicenseTypeIssuedDate { get; set; }
        [Required(ErrorMessage = "Available license type is required")]
        public string AvailableLicenseType { get; set; }
    }

    public static class LicenseRegisterFormRequestExtension 
    {
        public static LicenseRegisterFormModel ToLicenseFormRegisterModel(this LicenseRegisterFormRequest reqObj) 
        {
            return new LicenseRegisterFormModel {
                CreateDate = DateTime.Now,
                LicenseFormDesc = $"Tạo ngày {DateTime.Now}",
                LicenseTypeId = reqObj.LicenseTypeId,
                // default form status
                RegisterFormStatusId = 1,
                PermanentAddress = reqObj.PermanentAddress,
                IdentityNumber = reqObj.IdentityNumber,
                Gender = reqObj.Gender,
                LicenseTypeIssuedDate = DateTime.ParseExact(reqObj.LicenseTypeIssuedDate, 
                    "yyyy-MM-dd", CultureInfo.InvariantCulture),
                IdentityCardIssuedBy = reqObj.IdentityCardIssuedBy,
                IdentityCardIssuedDate = DateTime.ParseExact(reqObj.IdentityCardIssuedDate,
                    "yyyy-MM-dd", CultureInfo.InvariantCulture),
                AvailableLicenseType = reqObj.AvailableLicenseType
            };
        }
    }
}
