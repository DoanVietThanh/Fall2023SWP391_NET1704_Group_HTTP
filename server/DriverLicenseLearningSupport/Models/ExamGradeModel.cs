namespace DriverLicenseLearningSupport.Models
{
    public class ExamGradeModel
    {
        public string MemberId { get; set; }
        public int TheoryExamId { get; set; }
        public double? Point { get; set; }
        public int QuestionId { get; set; }
        public int SelectedAnswerId { get; set; }
        public string Email { get; set; }
    }
}
