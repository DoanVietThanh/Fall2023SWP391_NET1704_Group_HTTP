using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class TheoryExamModel
    {

        public int TheoryExamId { get; set; }
        public int? TotalQuestion { get; set; }
        public int? TotalTime { get; set; }
        public int? TotalAnswerRequired { get; set; }
        public int? LicenseTypeId { get; set; }
        public bool? IsMockExam { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeSpan? StartTime { get; set; }

        public virtual LicenseTypeModel LicenseType { get; set; }
        public virtual ICollection<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
    }
}
