using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Tag
    {
        public Tag()
        {
            Blogs = new HashSet<Blog>();
        }

        public int TagId { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
