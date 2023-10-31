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
        //public virtual LicenseType LicenseType { get; set; }
        public virtual ICollection<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
    }
}
