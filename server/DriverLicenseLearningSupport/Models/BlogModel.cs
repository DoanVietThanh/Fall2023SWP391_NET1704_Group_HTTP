using DriverLicenseLearningSupport.Entities;

namespace DriverLicenseLearningSupport.Models
{
    public class BlogModel
    {
        public int BlogId { get; set; }
        public string StaffId { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }

        public string Image { get;set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public virtual StaffModel Staff { get; set; }

        public virtual ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();

        public virtual ICollection<TagModel> Tags { get; set; } = new List<TagModel>();
    }
}
