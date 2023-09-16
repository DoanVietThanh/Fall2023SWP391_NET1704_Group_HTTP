using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class LicenseRegisterFormStatus
    {
        public LicenseRegisterFormStatus()
        {
            LicenseRegisterForms = new HashSet<LicenseRegisterForm>();
        }

        public int RegisterFormStatusId { get; set; }
        public string RegisterFormStatusDesc { get; set; }

        public virtual ICollection<LicenseRegisterForm> LicenseRegisterForms { get; set; }
    }
}
