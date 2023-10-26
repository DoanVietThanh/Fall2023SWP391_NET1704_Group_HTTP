using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class CoursePackage
    {
        public CoursePackage()
        {
            CoursePackageReservations = new HashSet<CoursePackageReservation>();
            TeachingSchedules = new HashSet<TeachingSchedule>();
        }

        public string CoursePackageId { get; set; }
        public string CoursePackageDesc { get; set; }
        public int? TotalSession { get; set; }
        public int? SessionHour { get; set; }
        public double? Cost { get; set; }
        public int? AgeRequired { get; set; }
        public string CourseId { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<CoursePackageReservation> CoursePackageReservations { get; set; }
        public virtual ICollection<TeachingSchedule> TeachingSchedules { get; set; }
    }
}
