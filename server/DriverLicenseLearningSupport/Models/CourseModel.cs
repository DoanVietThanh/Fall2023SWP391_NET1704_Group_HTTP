using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class CourseModel
    {
        public string CourseId { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public string CourseDesc { get; set; } = null!;
        public double Cost { get; set; } = 0;
        public int? TotalSession { get; set; } = null!;
        public int? TotalMonth { get; set; } = null!;
        public DateTime? StartDate { get; set; } = null!;
        public bool? IsActive { get; set; } = null!;
        public virtual ICollection<FeedBackModel> FeedBacks { get; set; } = new List<FeedBackModel>();
        public virtual ICollection<CurriculumModel> Curricula { get; set; } = new List<CurriculumModel>();
        public virtual ICollection<StaffModel> Mentors { get; set; } = new List<StaffModel>();
    }
}
