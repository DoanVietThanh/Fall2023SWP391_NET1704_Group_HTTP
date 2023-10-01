﻿using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class LicenseRegisterFormUpdate
    {
        [Required(ErrorMessage = "License register form id required")]
        public int LicenseFormId { get; set; }

        // privacy image
        public IFormFile? Image { get; set; } = null!;

        // identity image
        public IFormFile? IdentityImage { get; set; } = null!;
        
        // health certificate image
        public IFormFile? HealthCertificationImage { get; set; } = null!;

        [Required(ErrorMessage = "Member Id is required")]
        public Guid MemberId { get; set; }
        [Required(ErrorMessage = "License Type Id is required")]
        public int LicenseTypeId { get; set; }
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
            };
        }
    }
}