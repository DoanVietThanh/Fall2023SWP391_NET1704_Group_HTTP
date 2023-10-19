using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Course
    {
        public Course()
        {
            CoursePackages = new HashSet<CoursePackage>();
            FeedBacks = new HashSet<FeedBack>();
            WeekdaySchedules = new HashSet<WeekdaySchedule>();
            Curricula = new HashSet<Curriculum>();
            Mentors = new HashSet<Staff>();
        }

        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDesc { get; set; }
        public int? TotalMonth { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? IsActive { get; set; }
        public int? LicenseTypeId { get; set; }
        public int? TotalHoursRequired { get; set; }
        public int? TotalKmRequired { get; set; }

        public virtual LicenseType LicenseType { get; set; }
        public virtual ICollection<CoursePackage> CoursePackages { get; set; }
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
        public virtual ICollection<WeekdaySchedule> WeekdaySchedules { get; set; }

        public virtual ICollection<Curriculum> Curricula { get; set; }
        public virtual ICollection<Staff> Mentors { get; set; }
    }
}
