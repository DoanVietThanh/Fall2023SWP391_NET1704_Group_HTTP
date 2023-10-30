using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class CoursePackageModel
    {
        public string CoursePackageId { get; set; }
        public string CoursePackageDesc { get; set; }
        public int? TotalSession { get; set; }
        public int? SessionHour { get; set; }
        public double? Cost { get; set; }
        public int? AgeRequired { get; set;}
        public string CourseId { get; set; }

        public virtual CourseModel Course { get; set; }

        public virtual ICollection<TeachingScheduleModel> TeachingSchedules { get; set; }
            = new List<TeachingScheduleModel>();
        //public virtual ICollection<CoursePackageReservationModel> 
        //    CoursePackageReservations { get; set; } = new List<CoursePackageReservationModel>();    
    }
}
