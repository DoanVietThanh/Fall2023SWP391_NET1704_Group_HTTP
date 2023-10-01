using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class TeachingScheduleModel
    {
        public int TeachingScheduleId { get; set; }
        public DateTime TeachingDate { get; set; }
        public string StaffId { get; set; }
        public int WeekdayScheduleId { get; set; }
        public int SlotId { get; set; }
        
        public WeekdayScheduleModel WeekdaySchedule { get; set; }
        public StaffModel Staff { get; set; }
        public SlotModel Slot { get; set; }
        public virtual ICollection<RollCallBook> RollCallBooks { get; set; }
    }
}
