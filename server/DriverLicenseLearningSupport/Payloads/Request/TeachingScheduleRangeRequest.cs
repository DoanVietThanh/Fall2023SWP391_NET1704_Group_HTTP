using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class TeachingScheduleRangeRequest
    {
        [Required]
        public string CourseId { get; set; }
        [Required]
        public string MentorId { get; set; }
        [Required]
        public int SlotId { get; set; }
        [Required]
        public string Weekdays { get; set; }
    }

    public static class TeachingScheduleRangeRequestExtension
    {
        public static TeachingScheduleModel ToInitScheduleModel(this TeachingScheduleRangeRequest reqObj)
        {
            return new TeachingScheduleModel
            {
                StaffId = reqObj.MentorId,
            };
        }
    }
}
