﻿using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class StaffModel
    {
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public string Phone { get; set; }
        public bool? IsActive { get; set; }
        public string? AvatarImage { get; set; }
        public string Email { get; set; }
        public string AddressId { get; set; }
        public int? JobTitleId { get; set; }
        public int? LicenseTypeId { get; set; }

        public virtual AddressModel Address { get; set; }
        public virtual AccountModel EmailNavigation { get; set; }
        public virtual JobTitleModel JobTitle { get; set; }
        public virtual LicenseTypeModel LicenseType { get; set; }
    }
}