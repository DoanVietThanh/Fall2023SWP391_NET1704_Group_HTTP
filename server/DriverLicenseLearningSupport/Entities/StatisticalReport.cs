using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class StatisticalReport
    {
        public int ReportId { get; set; }
        public int? TotalMember { get; set; }
        public int? TotalMentor { get; set; }
        public int? TotalCourse { get; set; }
        public int? TotalCourseSchedule { get; set; }
        public int? TotalBlog { get; set; }
        public int? TotalRevenue { get; set; }
        public int? TotalActiveMember { get; set; }
    }
}
