using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Staff
    {
        public Staff()
        {
            Blogs = new HashSet<Blog>();
            CoursePackageReservations = new HashSet<CoursePackageReservation>();
            FeedBacks = new HashSet<FeedBack>();
            TeachingSchedules = new HashSet<TeachingSchedule>();
            Courses = new HashSet<Course>();
        }

        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public string Phone { get; set; }
        public bool? IsActive { get; set; }
        public string AvatarImage { get; set; }
        public string Email { get; set; }
        public string AddressId { get; set; }
        public int? JobTitleId { get; set; }
        public int? LicenseTypeId { get; set; }
        public string SeftDescription { get; set; }

        public virtual Address Address { get; set; }
        public virtual Account EmailNavigation { get; set; }
        public virtual JobTitle JobTitle { get; set; }
        public virtual LicenseType LicenseType { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<CoursePackageReservation> CoursePackageReservations { get; set; }
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
        public virtual ICollection<TeachingSchedule> TeachingSchedules { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
