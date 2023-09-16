using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class CourseReservationStatus
    {
        public CourseReservationStatus()
        {
            CourseReservations = new HashSet<CourseReservation>();
        }

        public int CourseReservationStatusId { get; set; }
        public string CourseReservationStatusDesc { get; set; }

        public virtual ICollection<CourseReservation> CourseReservations { get; set; }
    }
}
