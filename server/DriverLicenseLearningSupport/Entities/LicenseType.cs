﻿using Amazon.S3.Model;
using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class LicenseType
    {
        public LicenseType()
        {
            LicenseRegisterForms = new HashSet<LicenseRegisterForm>();
            Members = new HashSet<Member>();
            Questions = new HashSet<Question>();
            TheoryExams = new HashSet<TheoryExam>();
            Staffs = new HashSet<Staff>();
        }

        public int LicenseTypeId { get; set; }
        public string LicenseTypeDesc { get; set; }

        public virtual ICollection<LicenseRegisterForm> LicenseRegisterForms { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<TheoryExam> TheoryExams { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
    }
}