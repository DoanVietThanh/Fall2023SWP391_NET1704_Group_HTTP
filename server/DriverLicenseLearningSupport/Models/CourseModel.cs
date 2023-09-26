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
        public bool? IsActive { get; set; } = null!;

        public virtual ICollection<Curriculum> Curricula { get; set; } = new List<Curriculum>();
    }
}
