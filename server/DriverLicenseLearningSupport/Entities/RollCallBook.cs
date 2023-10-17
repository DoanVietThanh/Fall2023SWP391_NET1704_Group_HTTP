using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class RollCallBook
    {
        public int RollCallBookId { get; set; }
        public bool? IsAbsence { get; set; }
        public string? Comment { get; set; }
        public string MemberId { get; set; }
        public int? MemberTotalSession { get; set; }
        public int TeachingScheduleId { get; set; }
        public int? TotalHoursDriven { get; set; }
        public int? TotalKmDriven { get; set; }

        public virtual Member Member { get; set; }
        public virtual TeachingSchedule TeachingSchedule { get; set; }
    }
}
