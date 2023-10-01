using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class CourseReservation
    {
        public string CourseReservationId { get; set; }
        public DateTime CourseStartDate { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public string MemberId { get; set; }
        public string CourseId { get; set; }
        public string StaffId { get; set; }
        public int PaymentTypeId { get; set; }
        public double? PaymentAmmount { get; set; }
        public int? CourseReservationStatusId { get; set; }
        public int? VehicleId { get; set; }

        public virtual Course Course { get; set; }
        public virtual CourseReservationStatus CourseReservationStatus { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual Member Member { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
