namespace DriverLicenseLearningSupport.Models
{
    public class SlotModel
    {
        public int SlotId { get; set; }
        public string SlotName { get; set; }
        public int? Duration { get; set; }
        public TimeSpan? Time { get; set; }
        public string SlotDesc { get; set; }

        public virtual ICollection<TeachingScheduleModel> TeachingSchedules { get; set; } 
            = new List<TeachingScheduleModel>();
    }
}
