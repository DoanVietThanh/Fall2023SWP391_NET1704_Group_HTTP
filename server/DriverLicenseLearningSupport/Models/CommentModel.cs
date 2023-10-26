namespace DriverLicenseLearningSupport.Models
{
    public class CommentModel
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public int? BlogId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string AvatarImage { get; set; }
    }
}
