using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class LicenseRegisterFormUpdate
    {
        [Required(ErrorMessage = "License register form id required")]
        public int LicenseFormId { get; set; }

        // privacy image
        public IFormFile? Image { get; set; } = null!;

        // identity image
        public IFormFile? IdentityCardImage { get; set; } = null!;
        
        // health certificate image
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

    public static class LicenseFormRegisterUpdateExtension
    {
        public static LicenseRegisterFormModel ToLicenseFormRegisterModel(this LicenseRegisterFormUpdate reqObj)
        {
            return new LicenseRegisterFormModel
            {
                LicenseFormId = reqObj.LicenseFormId,
                LicenseFormDesc = $"Last modified date {DateTime.Now}",
                LicenseTypeId = reqObj.LicenseTypeId,
                Gender = reqObj.Gender,
                PermanentAddress = reqObj.PermanentAddress,
                IdentityNumber = reqObj.IdentityNumber,
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
