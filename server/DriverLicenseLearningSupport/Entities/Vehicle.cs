using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            CoursePackageReservations = new HashSet<CoursePackageReservation>();
            TeachingSchedules = new HashSet<TeachingSchedule>();
        }

        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleLicensePlate { get; set; }
        public DateTime? RegisterDate { get; set; }
        public int? VehicleTypeId { get; set; }
        public string VehicleImage { get; set; }

        public virtual VehicleType VehicleType { get; set; }
        public virtual ICollection<CoursePackageReservation> CoursePackageReservations { get; set; }
        public virtual ICollection<TeachingSchedule> TeachingSchedules { get; set; }
    }
}
