using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class ExamGradeModel
    {
        public int ExamGradeId { get; set; }
        public string MemberId { get; set; }
        public int TheoryExamId { get; set; }
        public double? Point { get; set; }
        public int QuestionId { get; set; }
        public int SelectedAnswerId { get; set; }
        public string Email { get; set; }
        public DateTime? StartDate { get; set; }
        public virtual QuestionModel Question { get; set; }
    }
}
