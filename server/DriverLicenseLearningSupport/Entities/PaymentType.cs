using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class PaymentType
    {
        public PaymentType()
        {
            CoursePackageReservations = new HashSet<CoursePackageReservation>();
        }

        public int PaymentTypeId { get; set; }
        public string PaymentTypeDesc { get; set; }

        public virtual ICollection<CoursePackageReservation> CoursePackageReservations { get; set; }
    }
}
