using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class TeachingScheduleSubstituteRequest
    {
        [Required]
        public string MentorId { get; set; }
        
        [Required]
        public DateTime TeachingDate { get; set; }

        [Required]
        public int SlotId { get; set; }

        [Required]
        public string SubstituteMentorId { get; set; }
    }
}
