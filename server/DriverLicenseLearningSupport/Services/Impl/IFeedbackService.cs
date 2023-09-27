using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Services.Impl
{
    public interface IFeedbackService
    {
        Task<bool> CreateAsync(FeedBackModel feedback);
        Task<IEnumerable<FeedBackModel>> GetAllMentorFeedback(Guid mentorId);
    }
}
