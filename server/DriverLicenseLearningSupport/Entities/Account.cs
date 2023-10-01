using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Account
    {
        public Account()
        {
            Members = new HashSet<Member>();
            Staffs = new HashSet<Staff>();
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
    }
}
