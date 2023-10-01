using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Address
    {
        public Address()
        {
            Members = new HashSet<Member>();
            Staffs = new HashSet<Staff>();
        }

        public string AddressId { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string? Zipcode { get; set; }

        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
    }
}
