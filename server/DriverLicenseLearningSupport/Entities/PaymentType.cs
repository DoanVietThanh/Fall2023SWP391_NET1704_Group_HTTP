using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class PaymentType
    {
        public PaymentType()
        {
            CourseReservations = new HashSet<CourseReservation>();
        }

        public int PaymentTypeId { get; set; }
        public string PaymentTypeDesc { get; set; }

        public virtual ICollection<CourseReservation> CourseReservations { get; set; }
    }
}
