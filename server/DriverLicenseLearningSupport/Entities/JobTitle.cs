using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class JobTitle
    {
        public JobTitle()
        {
            Staffs = new HashSet<Staff>();
        }

        public int JobTitleId { get; set; }
        public string JobTitleDesc { get; set; }

        public virtual ICollection<Staff> Staffs { get; set; }
    }
}
