using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Course
    {
        public Course()
        {
            CourseReservations = new HashSet<CourseReservation>();
            FeedBacks = new HashSet<FeedBack>();
            Curricula = new HashSet<Curriculum>();
        }

        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDesc { get; set; }
        public double Cost { get; set; }
        public int? TotalSession { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<CourseReservation> CourseReservations { get; set; }
        public virtual ICollection<FeedBack> FeedBacks { get; set; }

        public virtual ICollection<Curriculum> Curricula { get; set; }
    }
}
