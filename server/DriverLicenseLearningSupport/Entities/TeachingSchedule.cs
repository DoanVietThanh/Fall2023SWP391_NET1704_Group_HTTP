using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class TeachingSchedule
    {
        public TeachingSchedule()
        {
            RollCallBooks = new HashSet<RollCallBook>();
        }

        public int TeachingScheduleId { get; set; }
        public DateTime TeachingDate { get; set; }
        public string StaffId { get; set; }
        public int? SlotId { get; set; }
        public int? VehicleId { get; set; }
        public int? WeekdayScheduleId { get; set; }
        public string? CoursePackageId { get; set; } 
        public bool? IsActive { get; set; }
        public virtual CoursePackage? CoursePackage { get; set; }
        public virtual Slot Slot { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Vehicle? Vehicle { get; set; }
        public virtual WeekdaySchedule WeekdaySchedule { get; set; }
        public virtual ICollection<RollCallBook> RollCallBooks { get; set; }
    }
}
