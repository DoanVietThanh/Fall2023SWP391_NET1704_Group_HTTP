using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class LicenseRegisterForm
    {
        public LicenseRegisterForm()
        {
            Members = new HashSet<Member>();
        }

        public int LicenseFormId { get; set; }
        public string LicenseFormDesc { get; set; }
        public string Image { get; set; }
        public string IdentityCardImage { get; set; }
        public string HealthCertificationImage { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? RegisterFormStatusId { get; set; }
        public int? LicenseTypeId { get; set; }

        public virtual LicenseType LicenseType { get; set; }
        public virtual LicenseRegisterFormStatus RegisterFormStatus { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
