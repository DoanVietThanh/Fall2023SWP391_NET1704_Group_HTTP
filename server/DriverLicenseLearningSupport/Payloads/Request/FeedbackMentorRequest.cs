using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Payloads.Request
{
    public class FeedbackMentorRequest
    {
        // member Id
        public string? MemberId { get; set; }
        // mentor Id
        public string? MentorId { get; set; }
        // content
        public string? Content { get; set; }
        // rating star
        public int RatingStar { get; set; }
    }

    public static class FeedbackMentorRequestExtension
    {
        public static FeedBackModel ToFeedbackModel(this FeedbackMentorRequest reqObj)
        {
            return new FeedBackModel { 
                MemberId = reqObj.MemberId,
                StaffId = reqObj.MentorId,
                Content = reqObj.Content,
                RatingStar = reqObj.RatingStar
            };
        }
    }
}
