using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class PracticeExam
    {
        public PracticeExam()
        {
            ExamGrades = new HashSet<ExamGrade>();
            ExamHistories = new HashSet<ExamHistory>();
            Questions = new HashSet<QuestionBank>();
        }

        public int PracticeExamId { get; set; }
        public int? TotalQuestion { get; set; }
        public int? TotalTime { get; set; }
        public int? TotalAnswerRequired { get; set; }
        public int? LicenseTypeId { get; set; }

        public virtual LicenseType LicenseType { get; set; }
        public virtual ICollection<ExamGrade> ExamGrades { get; set; }
        public virtual ICollection<ExamHistory> ExamHistories { get; set; }

        public virtual ICollection<QuestionBank> Questions { get; set; }
    }
}
