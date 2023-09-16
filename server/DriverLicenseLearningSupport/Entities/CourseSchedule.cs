using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class CourseSchedule
    {
        public CourseSchedule()
        {
            RollCallBooks = new HashSet<RollCallBook>();
        }

        public int CourseScheduleId { get; set; }
        public DateTime TeachingDate { get; set; }
        public string StaffId { get; set; }
        public string CourseReservationId { get; set; }
        public int? VehicleId { get; set; }

        public virtual CourseReservation CourseReservation { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual ICollection<RollCallBook> RollCallBooks { get; set; }
    }
}
