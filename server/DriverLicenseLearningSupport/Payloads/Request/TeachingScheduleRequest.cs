using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class TeachingScheduleRequest
    {
        public DateTime TeachingDate { get; set; }
        public string MentorId { get; set; }
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
