using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Staff
    {
        public Staff()
        {
            Blogs = new HashSet<Blog>();
            CourseReservations = new HashSet<CourseReservation>();
            CourseSchedules = new HashSet<CourseSchedule>();
            FeedBacks = new HashSet<FeedBack>();
        }

        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public string Phone { get; set; }
        public bool? IsActive { get; set; }
        public string? AvatarImage { get; set; }
        public string Email { get; set; }
        public string AddressId { get; set; }
        public int? JobTitleId { get; set; }
        public int? LicenseTypeId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Account EmailNavigation { get; set; }
        public virtual JobTitle JobTitle { get; set; }
        public virtual LicenseType LicenseType { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }
        public virtual ICollection<CourseReservation> CourseReservations { get; set; }
        public virtual ICollection<CourseSchedule> CourseSchedules { get; set; }
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
    }
}
