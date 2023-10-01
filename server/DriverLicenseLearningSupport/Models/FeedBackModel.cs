using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class FeedBackModel
    {
        public int FeedbackId { get; set; }
        public string Content { get; set; }
        public int? RatingStar { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? MemberId { get; set; }
        public string? StaffId { get; set; }
        public string? CourseId { get; set; }

        public virtual CourseModel Course { get; set; }
        public virtual MemberModel Member { get; set; }
        public virtual StaffModel Staff { get; set; }
    }
}
