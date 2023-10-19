using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class RollCallBookModel
    {
        public int RollCallBookId { get; set; }
        public bool? IsAbsence { get; set; }
        public string? Comment { get; set; }
        public string MemberId { get; set; }
        public int TeachingScheduleId { get; set; }
        public int? TotalHoursDriven { get; set; }
        public int? TotalKmDriven { get; set; }

        public virtual MemberModel Member { get; set; }
        public virtual TeachingScheduleModel TeachingSchedule { get; set; }
    }
}
