using DocumentFormat.OpenXml.Drawing.Diagrams;
using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class BlogCreateRequest
    {
        public string? StaffId { get; set; }
        [Required(ErrorMessage ="Blog phải có content")]
        public string Content { get; set; }

        public string[] TagNames { get; set; }
    }
    public static class BlogCreateRequestExtension 
    {
        public static BlogModel ToBlogModel(this BlogCreateRequest request) 
        {
            return new BlogModel()
            {
                StaffId = request.StaffId,
                Content = HttpUtility.HtmlEncode(request.Content),
            };
        }
       public static IEnumerable<TagModel> ToListTagModels(this BlogCreateRequest request) 
        {
            List<TagModel> tags = new List<TagModel>();
            foreach (string TagName in request.TagNames)
            {
                TagModel tag = new TagModel()
                {
                    TagName = TagName,
                };
                tags.Add(tag);
            }
            return tags;
        }
    }
}
