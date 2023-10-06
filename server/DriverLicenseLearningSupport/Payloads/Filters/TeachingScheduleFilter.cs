namespace DriverLicenseLearningSupport.Payloads.Filters
{
    public class TeachingScheduleFilter
    {
        // filter by slot (if any come along with filter teaching-date/weekday)
        public int? SlotId { get; set; }

        // filter by particular teaching date
        public DateTime? TeachingDate { get; set; }

        // filter by weekday
        public int? WeekDayScheduleId { get; set; }
    }
}
