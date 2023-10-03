using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class CourseReservationRequest
    {
        [Required(ErrorMessage = "Member id is required")]
        public Guid MemberId { get; set; }

        [Required(ErrorMessage = "Mentor id is required")]
        public Guid MentorId { get; set; }
        
        [Required(ErrorMessage = "Course id is required")]
        public Guid CourseId { get; set; }

        [Required(ErrorMessage = "Payment type is required")]
        public int PaymentTypeId { get; set; }

        public double? PaymentAmount { get; set; } = 0;
    }

    public static class CourseReservationRequestExtension 
    {
        public static CourseReservationModel ToCourseReservationModel(this CourseReservationRequest reqObj)
        {
            return new CourseReservationModel { 
                CourseReservationId = Guid.NewGuid().ToString(),
                MemberId = reqObj.MemberId.ToString(),
                StaffId = reqObj.MentorId.ToString(),
                CourseId = reqObj.CourseId.ToString(),
                PaymentTypeId = reqObj.PaymentTypeId,
                PaymentAmmount = reqObj.PaymentAmount,
                // reservation status <- default is not payment yet
                CourseReservationStatusId = 1
            };
        }
    }
}
