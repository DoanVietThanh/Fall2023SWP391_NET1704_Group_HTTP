using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class TeachingScheduleRequest
    {
        [Required]
        public DateTime TeachingDate { get; set; }
        [Required]
        public string CourseId { get; set; }
        [Required]
        public string MentorId { get; set; }
        [Required]
        public int SlotId { get; set; }
    }

    public static class TeachingScheduleRequestExtension
    {
        public static TeachingScheduleModel ToScheduleModel(this TeachingScheduleRequest reqObj) 
        {
            return new TeachingScheduleModel { 
                TeachingDate = reqObj.TeachingDate,
                StaffId = reqObj.MentorId,
                SlotId = reqObj.SlotId
            };
        }
    }
}
