using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Member
    {
        public Member()
        {
            CoursePackageReservations = new HashSet<CoursePackageReservation>();
            ExamGrades = new HashSet<ExamGrade>();
            ExamHistories = new HashSet<ExamHistory>();
            FeedBacks = new HashSet<FeedBack>();
            RollCallBooks = new HashSet<RollCallBook>();
        }

        public string MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateBirth { get; set; }
        public string Phone { get; set; }
        public bool? IsActive { get; set; }
        public string AvatarImage { get; set; }
        public string AddressId { get; set; }
        public string Email { get; set; }
        public int? LicenseFormId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Account EmailNavigation { get; set; }
        public virtual LicenseRegisterForm? LicenseForm { get; set; }
        public virtual ICollection<CoursePackageReservation> CoursePackageReservations { get; set; }
        public virtual ICollection<ExamGrade> ExamGrades { get; set; }
        public virtual ICollection<ExamHistory> ExamHistories { get; set; }
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
        public virtual ICollection<RollCallBook> RollCallBooks { get; set; }
    }
}
