using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class RollCallBook
    {
        public int RollCallBookId { get; set; }
        public bool? IsAbsence { get; set; }
        public string Comment { get; set; }
        public string MemberId { get; set; }
        public int CourseScheduleId { get; set; }

        public virtual CourseSchedule CourseSchedule { get; set; }
        public virtual Member Member { get; set; }
    }
}
