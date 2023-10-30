using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class LicenseType
    {
        public LicenseType()
        {
            Courses = new HashSet<Course>();
            LicenseRegisterForms = new HashSet<LicenseRegisterForm>();
            Questions = new HashSet<Question>();
            SimulationSituations = new HashSet<SimulationSituation>();
            TheoryExams = new HashSet<TheoryExam>();
        }

        public int LicenseTypeId { get; set; }
        public string LicenseTypeDesc { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<LicenseRegisterForm> LicenseRegisterForms { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<SimulationSituation> SimulationSituations { get; set; }
        public virtual ICollection<TheoryExam> TheoryExams { get; set; }
    }
}
