using DriverLicenseLearningSupport.Models;
using System.Web;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class CreateNewCommentRequest
    {
        public string Content { get; set; }
        public int? BlogId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string AvatarImage { get; set; }
    }
    public static class CreateNewCommentExtension 
    {
        public static CommentModel ToCommentModel(this CreateNewCommentRequest request) 
        {
            return new CommentModel
            {
                Content =HttpUtility.HtmlEncode( request.Content),
                BlogId = request.BlogId,
                Email = request.Email,
                FullName = request.FullName,
            };
        }
    }
}
