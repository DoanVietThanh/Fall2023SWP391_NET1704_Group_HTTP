using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class WeekdayScheduleModel
    {
        public int WeekdayScheduleId { get; set; }
        public DateTime Monday { get; set; }
        public DateTime Tuesday { get; set; }
        public DateTime Wednesday { get; set; }
        public DateTime Thursday { get; set; }
        public DateTime Friday { get; set; }
        public DateTime Saturday { get; set; }
        public DateTime Sunday { get; set; }
        public string CourseId { get; set; }
        public string WeekdayScheduleDesc { get; set; }

        public virtual CourseModel Course { get; set; }
        public virtual ICollection<TeachingScheduleModel> TeachingSchedules { get; set; }
    }
}
