using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class ExamGrade
    {
        public string MemberId { get; set; }
        public int TheoryExamId { get; set; }
        public double? Point { get; set; }
        public int QuestionId { get; set; }
        public int SelectedAnswerId { get; set; }
        public string Email { get; set; }

        public virtual Member Member { get; set; }
        public virtual Question Question { get; set; }
        public virtual TheoryExam TheoryExam { get; set; }
    }
}
