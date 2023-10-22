using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class LearningScheduleRequest
    {
        [Required]
        public Guid MemberId { get; set; }
        //[Required]
        //public DateTime LearningDate { get; set; }
        //[Required]
        //public int SlotId { get; set; }
        [Required]
        public int TeachingScheduleId { get; set; }
    }

    public static class LearningScheduleRequestExtension
    {
        public static RollCallBookModel ToRollCallBookModel(this LearningScheduleRequest reqObj)
        {
            return new RollCallBookModel { 
                MemberId = reqObj.MemberId.ToString(),
                //TeachingScheduleId = reqObj.TeachingScheduleId,
                IsAbsence = true,
                IsActive = true
            };
        }
    }
}
