using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class ReservationStatus
    {
        public ReservationStatus()
        {
            CoursePackageReservations = new HashSet<CoursePackageReservation>();
        }

        public int ReservationStatusId { get; set; }
        public string ReservationStatusDesc { get; set; }

        public virtual ICollection<CoursePackageReservation> CoursePackageReservations { get; set; }
    }
}
