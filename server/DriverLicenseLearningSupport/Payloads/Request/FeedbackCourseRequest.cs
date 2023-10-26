using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class FeedbackCourseRequest
    {
        // member Id
        public string? MemberId { get; set; }
        // course Id
        public string? CourseId { get; set; }
        // content
        [Required(ErrorMessage = "Please input feedback content")]
        public string Content { get; set; }
        // rating star
        public int RatingStar { get; set; }
    }

    public static class FFeedbackCourseRequestExtension
    {
        public static FeedBackModel ToFeedbackModel(this FeedbackCourseRequest reqObj)
        {
            var currTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return new FeedBackModel
            {
                MemberId = reqObj.MemberId,
                CourseId = reqObj.CourseId,
                Content = reqObj.Content,
                RatingStar = reqObj.RatingStar,
                CreateDate = DateTime.ParseExact(currTime,
                "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
            };
        }
    }
}
