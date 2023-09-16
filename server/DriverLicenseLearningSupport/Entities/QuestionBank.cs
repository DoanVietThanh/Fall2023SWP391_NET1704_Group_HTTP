using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class QuestionBank
    {
        public QuestionBank()
        {
            ExamGrades = new HashSet<ExamGrade>();
            QuestionAnswers = new HashSet<QuestionAnswer>();
            PracticeExams = new HashSet<PracticeExam>();
        }

        public int QuestionId { get; set; }
        public string QuestionAnswerDesc { get; set; }
        public bool? IsParalysis { get; set; }
        public string Image { get; set; }
        public int? LicenseTypeId { get; set; }

        public virtual LicenseType LicenseType { get; set; }
        public virtual ICollection<ExamGrade> ExamGrades { get; set; }
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }

        public virtual ICollection<PracticeExam> PracticeExams { get; set; }
    }
}
