using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class CourseModel
    {
        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDesc { get; set; }
        public int? TotalMonth { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? IsActive { get; set; }
        public int? LicenseTypeId { get; set; }
        public int? TotalHoursRequired { get; set; }
        public int? TotalKmRequired { get; set; }

        public virtual LicenseTypeModel LicenseType { get; set; }
        public virtual ICollection<CoursePackageModel> CoursePackages { get; set; }
        public virtual ICollection<FeedBackModel> FeedBacks { get; set; }
        public virtual ICollection<CurriculumModel> Curricula { get; set; }
        public virtual ICollection<StaffModel> Mentors { get; set; }
    }
}
