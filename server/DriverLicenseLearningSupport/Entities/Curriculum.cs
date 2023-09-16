using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Curriculum
    {
        public Curriculum()
        {
            Courses = new HashSet<Course>();
        }

        public int CurriculumId { get; set; }
        public string CurriculumDesc { get; set; }
        public string CurriculumDetail { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
