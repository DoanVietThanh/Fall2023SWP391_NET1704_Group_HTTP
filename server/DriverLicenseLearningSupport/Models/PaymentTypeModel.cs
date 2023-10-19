using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class PaymentTypeModel
    {
        public int PaymentTypeId { get; set; }
        public string PaymentTypeDesc { get; set; }

        public virtual ICollection<CoursePackageReservation> CourseReservations { get; set; } 
            = new List<CoursePackageReservation>(); 
    }
}
