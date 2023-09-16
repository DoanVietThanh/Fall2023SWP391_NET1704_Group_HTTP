using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class LicenseType
    {
        public LicenseType()
        {
            LicenseRegisterForms = new HashSet<LicenseRegisterForm>();
            Members = new HashSet<Member>();
            PracticeExams = new HashSet<PracticeExam>();
            QuestionBanks = new HashSet<QuestionBank>();
            Staffs = new HashSet<Staff>();
        }

        public int LicenseTypeId { get; set; }
        public string LicenseTypeDesc { get; set; }

        public virtual ICollection<LicenseRegisterForm> LicenseRegisterForms { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<PracticeExam> PracticeExams { get; set; }
        public virtual ICollection<QuestionBank> QuestionBanks { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
    }
}
