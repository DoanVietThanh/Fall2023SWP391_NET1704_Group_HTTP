using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Slot
    {
        public Slot()
        {
            TeachingSchedules = new HashSet<TeachingSchedule>();
        }

        public int SlotId { get; set; }
        public string SlotName { get; set; }
        public int? Duration { get; set; }
        public TimeSpan? Time { get; set; }
        public string SlotDesc { get; set; }

        public virtual ICollection<TeachingSchedule> TeachingSchedules { get; set; }
    }
}
