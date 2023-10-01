using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            CourseReservations = new HashSet<CourseReservation>();
        }

        public int VehicleId { get; set; }
        public string VehicleLicensePlate { get; set; }
        public DateTime? RegisterDate { get; set; }
        public int? VehicleTypeId { get; set; }

        public virtual VehicleType VehicleType { get; set; }
        public virtual ICollection<CourseReservation> CourseReservations { get; set; }
    }
}
