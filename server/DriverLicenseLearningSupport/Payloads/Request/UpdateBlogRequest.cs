namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class UpdateBlogRequest
    {
        public int BlogId { get; set; }
        public IFormFile? Image { get; set; }
        public int[] TagIds { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
