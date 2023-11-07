using DocumentFormat.OpenXml.Drawing.Diagrams;
using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class BlogCreateRequest
    {
        public string? StaffId { get; set; }
        [Required(ErrorMessage ="Bài đăng phải có nội dung")]
        public string Content { get; set; }
        
        public IFormFile? Image { get; set; }
        [Required(ErrorMessage = "Bài đăng phải có nội dung")]
        public string Title { get; set; }
        public int[] TagIds { get; set; }
    }
    public static class BlogCreateRequestExtension 
    {
        public static BlogModel ToBlogModel(this BlogCreateRequest request) 
        {
            return new BlogModel()
            {
                StaffId = request.StaffId,
                Content = HttpUtility.HtmlEncode(request.Content),
                Title = HttpUtility.HtmlEncode(request.Title)
            };
        }
       //public static IEnumerable<TagModel> ToListTagModels(this BlogCreateRequest request) 
       // {
       //     List<TagModel> tags = new List<TagModel>();
       //     foreach (int id in request.TagIds)
       //     {
       //         TagModel tag = new TagModel()
       //         {
       //             TagId = id,
       //         };
       //         tags.Add(tag);
       //     }
       //     return tags;
       // }
    }
}
