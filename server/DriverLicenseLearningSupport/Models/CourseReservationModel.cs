using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class CourseReservationModel
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

        public virtual CourseModel Course { get; set; }
        //public virtual CourseReservationStatusModel CourseReservationStatus { get; set; }
        public virtual PaymentTypeModel PaymentType { get; set; }
        public virtual MemberModel Member { get; set; }
        public virtual StaffModel Staff { get; set; }
        public virtual VehicleModel Vehicle { get; set; }
    }
}
