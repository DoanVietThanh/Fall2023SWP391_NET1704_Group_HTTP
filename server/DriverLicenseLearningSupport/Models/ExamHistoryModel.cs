using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class ExamHistoryModel
    {
        public int ExamHistoryId { get; set; }
        public string MemberId { get; set; }
        public int TheoryExamId { get; set; }
        public int? TotalGrade { get; set; }
        public int? TotalRightAnswer { get; set; }
        public int? TotalQuestion { get; set; }
        public int? TotalTime { get; set; }
        public bool? WrongParalysisQuestion { get; set; }
        public bool? IsPassed { get; set; }
        public string? TheoryExamDesc { get; set; }

        public DateTime? Date { get; set; }
        public virtual MemberModel Member { get; set; }
        public virtual TheoryExamModel TheoryExam { get; set; }
    }
}
