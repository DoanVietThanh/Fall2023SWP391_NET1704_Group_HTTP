using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Invoice
    {
        public Invoice()
        {
            CourseReservations = new HashSet<CourseReservation>();
        }

        public int InvoiceId { get; set; }
        public double? Ammount { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? PaymentTypeId { get; set; }

        public virtual PaymentType PaymentType { get; set; }
        public virtual ICollection<CourseReservation> CourseReservations { get; set; }
    }
}
