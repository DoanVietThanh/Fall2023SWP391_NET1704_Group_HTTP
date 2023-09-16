﻿using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class ExamHistory
    {
        public string MemberId { get; set; }
        public int PracticeExamId { get; set; }
        public int? TotalGrade { get; set; }
        public int? TotalRightAnswer { get; set; }
        public int? TotalQuestion { get; set; }
        public int? TotalTime { get; set; }
        public bool? WrongParalysisQuestion { get; set; }
        public bool? IsPassed { get; set; }

        public virtual Member Member { get; set; }
        public virtual PracticeExam PracticeExam { get; set; }
    }
}