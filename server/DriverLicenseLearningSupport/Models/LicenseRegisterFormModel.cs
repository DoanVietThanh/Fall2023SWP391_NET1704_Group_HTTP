using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class LicenseRegisterFormModel
    {
        public int LicenseFormId { get; set; }
        public string LicenseFormDesc { get; set; }
        public string Image { get; set; }
        public string IdentityCardImage { get; set; }
        public string HealthCertificationImage { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? RegisterFormStatusId { get; set; }
        public int? LicenseTypeId { get; set; }

        public virtual LicenseTypeModel LicenseType { get; set; }
        public virtual LicenseRegisterFormStatusModel RegisterFormStatus { get; set; }
    }
}
