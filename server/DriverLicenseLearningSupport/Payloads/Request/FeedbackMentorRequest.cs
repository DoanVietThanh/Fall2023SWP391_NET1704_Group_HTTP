using DocumentFormat.OpenXml.VariantTypes;
using DriverLicenseLearningSupport.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class FeedbackMentorRequest
    {
        // member Id
        public string? MemberId { get; set; }
        // mentor Id
        public string? MentorId { get; set; }
        // content
        [Required(ErrorMessage = "Please input feedback content")]
        public string Content { get; set; }
        // rating star
        public int RatingStar { get; set; }
    }

    public static class FeedbackMentorRequestExtension
    {
        public static FeedBackModel ToFeedbackModel(this FeedbackMentorRequest reqObj)
        {
            var currTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return new FeedBackModel { 
                MemberId = reqObj.MemberId,
                StaffId = reqObj.MentorId,
                Content = reqObj.Content,
                RatingStar = reqObj.RatingStar,
                CreateDate = DateTime.ParseExact(currTime,
                "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
            };
        }
    }
}
