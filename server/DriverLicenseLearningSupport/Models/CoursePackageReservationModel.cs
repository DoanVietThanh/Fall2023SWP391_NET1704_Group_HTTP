using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class CoursePackageReservationModel
    {
        public string CoursePackageReservationId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string MemberId { get; set; }
        public string CoursePackageId { get; set; }
        public string StaffId { get; set; }
        public int? ReservationStatusId { get; set; }
        public int PaymentTypeId { get; set; }
        public double? PaymentAmmount { get; set; }
        public int? VehicleId { get; set; }

        public virtual CoursePackageModel CoursePackage { get; set; }
        public virtual MemberModel Member { get; set; }
        public virtual PaymentTypeModel PaymentType { get; set; }
        public virtual ReservationStatusModel ReservationStatus { get; set; }
        public virtual StaffModel Staff { get; set; }
        public virtual VehicleModel Vehicle { get; set; }
    }
}
