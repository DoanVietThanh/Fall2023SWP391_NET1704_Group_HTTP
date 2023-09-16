using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            CourseSchedules = new HashSet<CourseSchedule>();
        }

        public int VehicleId { get; set; }
        public string VehicleLicensePlate { get; set; }
        public DateTime? RegisterDate { get; set; }
        public int? VehicleTypeId { get; set; }

        public virtual VehicleType VehicleType { get; set; }
        public virtual ICollection<CourseSchedule> CourseSchedules { get; set; }
    }
}
