using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class CoursePackageReservation
    {
        public string CoursePackageReservationId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string MemberId { get; set; }
        public string CoursePackageId { get; set; }
        public string StaffId { get; set; }
        public int? ReservationStatusId { get; set; }
        public int PaymentTypeId { get; set; }
        public double? PaymentAmmount { get; set; }

        public virtual CoursePackage CoursePackage { get; set; }
        public virtual Member Member { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual ReservationStatus ReservationStatus { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
