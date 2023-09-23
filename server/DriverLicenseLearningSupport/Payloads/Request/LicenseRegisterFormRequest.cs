using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Services.Impl;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class LicenseRegisterFormRequest
    {
        [Required(ErrorMessage = "Image is required")]
        public IFormFile Image { get; set; } = null!;

        [Required(ErrorMessage = "Identity Image is required")]
        public IFormFile IdentityImage { get; set; } = null!;
        [Required(ErrorMessage = "Health certification Image is required")]
        public IFormFile HealthCertificationImage { get; set; } = null!;

        //public DateTime? CreateDate { get; set; } = DateTime.Now;
        public string? LicenseFormDesc { get; set; } = null!;

        [Required(ErrorMessage = "Member Id is required")]
        public Guid MemberId { get; set; }
        [Required(ErrorMessage = "License Type Id is required")]
        public int LicenseTypeId { get; set; }
    }

    public static class LicenseRegisterFormRequestExtension 
    {
        public static LicenseRegisterFormModel ToLicenseFormRegisterModel(this LicenseRegisterFormRequest reqObj) 
        {
            return new LicenseRegisterFormModel {
                Image = Guid.NewGuid().ToString(),
                IdentityCardImage = Guid.NewGuid().ToString(),
                HealthCertificationImage = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                LicenseFormDesc = $"Create at {DateTime.Now}",
                LicenseTypeId = reqObj.LicenseTypeId,
                RegisterFormStatusId = 1
            };
        }
    }
}
