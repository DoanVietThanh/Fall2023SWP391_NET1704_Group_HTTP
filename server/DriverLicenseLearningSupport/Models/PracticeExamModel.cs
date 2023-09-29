using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class PracticeExamModel
    {
        public int PracticeExamId { get; set; }
        public int? TotalQuestion { get; set; }
        public int? TotalTime { get; set; }
        public int? TotalAnswerRequired { get; set; }
        public int? LicenseTypeId { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
