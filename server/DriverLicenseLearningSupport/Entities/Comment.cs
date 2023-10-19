using System;
using System.Collections.Generic;

namespace DriverLicenseLearningSupport.Entities
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public int? BlogId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string AvatarImage { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
