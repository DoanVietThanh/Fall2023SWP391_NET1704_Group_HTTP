using Amazon.S3.Model;
using System.ComponentModel.DataAnnotations;

namespace DriverLicenseLearningSupport.Payloads.Filters
{
    public class LearningScheduleFilter
    {
        // filter by slot (if any come along with filter teaching-date/weekday)
        public int? SlotId { get; set; }

        // filter by particular teaching date
        public DateTime? LearningDate { get; set; }

        // filter by weekday
        public int? WeekDayScheduleId { get; set; }

        //// member Id
        //public Guid MemberId { get; set; }

        // MentorId
        [Required(ErrorMessage = "Mentor id is required")]
        public Guid MentorId { get; set; }
    }
}
