using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class CoursePackageReservationRequest
    {
        [Required(ErrorMessage = "Member id is required")]
        public Guid MemberId { get; set; }

        [Required(ErrorMessage = "Mentor id is required")]
        public Guid MentorId { get; set; }
        
        [Required(ErrorMessage = "Course Package id is required")]
        public Guid CoursePackageId { get; set; }

        [Required(ErrorMessage = "Payment type is required")]
        public int PaymentTypeId { get; set; }
    }

    public static class CoursePackageReservationRequestExtension 
    {
        public static CoursePackageReservationModel ToCoursePackageReservationModel(this CoursePackageReservationRequest reqObj)
        {
            return new CoursePackageReservationModel { 
                CoursePackageReservationId = Guid.NewGuid().ToString(),
                MemberId = reqObj.MemberId.ToString(),
                StaffId = reqObj.MentorId.ToString(),
                CoursePackageId = reqObj.CoursePackageId.ToString(),
                PaymentTypeId = reqObj.PaymentTypeId,
                // reservation status <- default is not payment yet
                ReservationStatusId = 1
            };
        }
    }
}
