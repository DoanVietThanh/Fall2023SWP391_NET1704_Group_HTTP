﻿using DriverLicenseLearningSupport.Entities;
using DriverLicenseLearningSupport.Models;

namespace DriverLicenseLearningSupport.Repositories.Impl
{
    public interface IFeedbackRepository
    {
        Task<bool> CreateAsync(FeedBack feedback);
        Task<IEnumerable<FeedBackModel>> GetAllMentorFeedback(Guid mentorId);
        Task<IEnumerable<FeedBackModel>> GetAllCourseFeedback(Guid courseId);
        Task<bool> DeleteAsync(int id);
    }
}
